using EmployeeLibrary.Utilities;
using EmployeeMVC.Models;
using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Research.SEAL;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMVC.Controllers
{
    public class TestMVCController : Controller
    {
        private readonly Utilities _utilities;
        private readonly HttpClient _http;
        public TestMVCController()
        {
            _utilities = new Utilities();
            _http = new HttpClient();
            _http.BaseAddress = new System.Uri("https://localhost:44350/api/testapi/");
        }
        [HttpGet]
        public IActionResult Input()
        {
            InputValue inputValue = new InputValue();
            return View(inputValue);
        }
        [HttpPost]
        public async Task<IActionResult> Input(InputValue inputValue)
        {
            Ciphertext cipherValue1 = _utilities.DoubleToCiphertext(inputValue.Value1);
            Ciphertext cipherValue2 = _utilities.DoubleToCiphertext(inputValue.Value2);
            string stringValue1 = _utilities.CiphertextToBase64String(cipherValue1);
            string stringValue2 = _utilities.CiphertextToBase64String(cipherValue2);
            CipherValues cipherValues = new CipherValues()
            {
                Value1 = stringValue1,
                Value2 = stringValue2
            };
            StringContent content= new StringContent(JsonConvert.SerializeObject(cipherValues),Encoding.UTF8,"application/json");
            HttpResponseMessage res=await _http.PostAsync("",content);
            if (res.IsSuccessStatusCode)
            {
                var result=res.Content.ReadAsStringAsync().Result;
                CipherValues values= JsonConvert.DeserializeObject<CipherValues>(result);
                Ciphertext stringCipherToCipher = _utilities.BuildCiphertextFromBase64String(values.Result);
                double val = _utilities.CiphertextToDouble(stringCipherToCipher);
                ViewData["response"] = val;
                return View("~/Views/Shared/Success.cshtml");
            }
            return View(inputValue);
            
        }
    }
}
