using AppMercadoPago_Youtube.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MercadoPago.Config;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace AppMercadoPago_Youtube.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EnviarPagoDto modelo)
        {
            if(modelo.IdentificationType is null)
            {
                modelo.IdentificationType = "CC";
            }
            var accessToken = obtenerAccessToken();
            var publicKey = obtenerPublicKey();

            //*Mercado pago configuration
            MercadoPagoConfig.AccessToken = accessToken;
            var productDetails = new Products()
            {
                Id = 1,
                Name = "Xiaomi C15",
                Unit = 2,
                UnitPrice = 30000,
                CategoryId = 8,
                Description = "The new era of the intelligence",
            };

            //*Crear reques hacia mercado pago
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Id = productDetails.Id.ToString(),
                        Title = productDetails.Name,
                        CurrencyId = "COP",
                        PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                        Description = productDetails.Description,
                        CategoryId = productDetails.CategoryId.ToString(),
                        Quantity = productDetails.Unit,
                        UnitPrice = Convert.ToDecimal(productDetails.UnitPrice)
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = modelo.Email,
                    Surname = modelo.Email.ToUpper(),
                    Email = modelo.Email,
                    Identification = new IdentificationRequest
                    {
                        Type = modelo.IdentificationType,
                        Number = modelo.IdentificationNumber
                    },
                    Address = new AddressRequest
                    {
                        StreetName = modelo.StreetName,
                        StreetNumber = modelo.StreetNumber,
                        ZipCode = modelo.ZipCode
                    },
                    Phone = new PhoneRequest
                    {
                        AreaCode = modelo.PhoneAreaCode,
                        Number = modelo.PhoneNumber
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://localhost:7273/Success",
                    Failure = "https://localhost:7273/Failure",
                },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = [],
                    ExcludedPaymentTypes = [],
                    Installments = 8
                },
                StatementDescriptor = "Sistema de Ventas .net 8",
                ExternalReference = $"Referencia_{Guid.NewGuid().ToString()}",
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddMinutes(10)
            };
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            return Redirect(preference.SandboxInitPoint);
            //return Redirect(preference.InitPoint);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("Success")]
        public async Task<IActionResult> Success([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
        [HttpGet("Failure")]
        public async Task<IActionResult> Failure([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Datos de configuracion para Mercado Pago
        private string obtenerAccessToken()
        {
            var token = _configuration.GetValue<string>("MercadoPago:AccessToken");
            return token.ToString() ?? string.Empty;
        }
        private string obtenerPublicKey()
        {
            var x = _configuration.GetValue<string>("MercadoPago:PublicKey");
            return x.ToString() ?? string.Empty;
        }
        #endregion
    }
}
