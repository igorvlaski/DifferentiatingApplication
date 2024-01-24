using DifferentiatingApplication.Domain;
using DifferentiatingApplication.DTO;
using DifferentiatingApplication.Persistence;

namespace DifferentiatingApplication.Services
{

    public class DiffService : IDiffService
    {
        private readonly DiffDbContext _dbContext;

        public DiffService(DiffDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Saves the data to the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="side"></param>
        /// <param name="data"></param>
        public void SaveData(int id, string side, byte[] data)
        {
            var entity = _dbContext.DataRecords.FirstOrDefault(x => x.Id == id && x.Side == side);

            if (entity == null)
            {
                entity = new DataRecord { Id = id, Side = side, Data = data };
                _dbContext.DataRecords.Add(entity);
            }
            else
            {
                entity.Data = data;
            }

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Gets the diff result
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetDiff(int id)
        {
            var left = _dbContext.DataRecords.FirstOrDefault(e => e.Id == id && e.Side == "left")?.Data;
            var right = _dbContext.DataRecords.FirstOrDefault(e => e.Id == id && e.Side == "right")?.Data;

            if (left == null || right == null)
            {
                return null;  
            }

            if (left.SequenceEqual(right))
            {
                return new { DiffResultType = "Equals" };
            }

            if (left.Length != right.Length)
            {
                return new { DiffResultType = "SizeDoNotMatch" };
            }

            var diffs = FindDiffs(left, right);

            return new
            {
                DiffResultType = "ContentDoNotMatch",
                Diffs = diffs
            };
        }

        /// <summary>
        /// Finds the diffs between the two byte arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static List<DiffDetail> FindDiffs(byte[] left, byte[] right)
        {
            var diffs = new List<DiffDetail>();
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    int diffStart = i;
                    while (i < left.Length && left[i] != right[i])
                    {
                        i++;
                    }
                    diffs.Add(new DiffDetail { Offset = diffStart, Length = i - diffStart });
                }
            }
            return diffs;
        }
    }
}
