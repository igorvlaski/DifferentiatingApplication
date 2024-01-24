using DifferentiatingApplication.DTO;
using DifferentiatingApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace DifferentiatingApplication.Controllers
{
    /// <summary>
    /// Controller for handling requests related to data differences.
    /// </summary>
    [ApiController]
    [Route("v1/diff/{id}")]
    public class DiffController : ControllerBase
    {
        private readonly IDiffService _diffService;

        /// <summary>
        /// Initializes a new instance of the DiffController.
        /// </summary>
        /// <param name="diffService">The service for handling diff-related operations.</param>

        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        /// <summary>
        /// Stores the given data on the specified side left.
        /// </summary>
        /// <param name="id">The ID of the diff record.</param>
        /// <param name="dataModel">The data to be stored, in base64 format.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPut("left")]
        public ActionResult PutLeftData(int id, [FromBody] DataModel dataModel)
        {
            return PutData(id, "left", dataModel);
        }

        /// <summary>
        /// Stores the given data on the specified side right.
        /// </summary>
        /// <param name="id">The ID of the diff record.</param>
        /// <param name="dataModel">The data to be stored, in base64 format.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPut("right")]
        public ActionResult PutRightData(int id, [FromBody] DataModel dataModel)
        {
            return PutData(id, "right", dataModel);
        }

        /// <summary>
        /// Stores the given data on the specified side.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="side"></param>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        private ActionResult PutData(int id, string side, DataModel dataModel)
        {
            if (!TryDecodeBase64(dataModel.Data, out var decodedData))
            {
                return BadRequest("Invalid base64 data.");
            }

            _diffService.SaveData(id, side, decodedData);
            return StatusCode(201); // Created
        }

        /// <summary>
        /// Gets the diff result for the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDiff(int id)
        {
            var diffResult = _diffService.GetDiff(id);

            if (diffResult == null)
            {
                return NotFound();
            }

            return Ok(diffResult);
        }

        /// <summary>
        /// Tries to decode the given base64 string into a byte array.
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryDecodeBase64(string base64, out byte[] data)
        {
            try
            {
                data = Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                data = null;
                return false;
            }
        }
    }
}
