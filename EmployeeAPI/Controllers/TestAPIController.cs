using EmployeeLibrary.Utilities;
using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Research.SEAL;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        private readonly Utilities _utilities;
        public TestAPIController()
        {
            _utilities=new Utilities();
        }
        [HttpPost]
        [Route("")]
        public IActionResult Multiply(CipherValues values)
        {
            Ciphertext val1 = _utilities.BuildCiphertextFromBase64String(values.Value1);
            Ciphertext val2 = _utilities.BuildCiphertextFromBase64String(values.Value2);
            double v1=_utilities.CiphertextToDouble(val1);
            double v2=_utilities.CiphertextToDouble(val2);
            Ciphertext val3=new Ciphertext();
            _utilities.Evaluator.Multiply(val1, val2, val3);
            values.Result = _utilities.CiphertextToBase64String(val3);
            return Ok(values);
        }
    }
}
