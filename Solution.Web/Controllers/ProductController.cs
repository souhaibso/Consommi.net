using Solution.Service;
using Solution.Domain.Entities;
using Solution.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using Solution.Data;
using System.Net.Mail;
using System.Net;

namespace Consomi.Web.Controllers
{
    public class ProductController : Controller
    {

        MyContext db = new MyContext();
        IProductService serviceProd;
        ICartService serviceCart;
        ICartLineService serviceCartLine;

        ApplicationUser user;

        public ProductController()
        {
            serviceProd = new ProductService();
            serviceCart = new CartService();
            serviceCartLine = new CartLineService();



        }

        // GET: Product
        public ActionResult Index()
        {
            var productList = serviceProd.GetAll();

            return View(productList);


        }

        //// GET: Product/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Product/Create7
        [NonAction]
        public void SendVerificationLinkEmail(string email, ICollection<CartLine> p)
        {
            String nomProd = "";
            int quantity=0;
            double price = 0;

            foreach (var c in p)
            {
                nomProd = c.myProduct.nom ;
                quantity += c.myProduct.quantite;
                price += c.prixTotal;
                
            };

            var fromEmail = new MailAddress("hsine.gabsi@esprit.tn", "Command Delivery");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Brigade2001";
            string subject = "Command Delivery";
            string body = "<br/><br>We are excited to tell you that your command " +
                             "will arrive in " + DateTime.Now.ToString("dd-MM-yyy") +
                             "<br/> Product Name : "+nomProd +
                             "<br/> Product Quantity : " + quantity+
                              "<br/> Product Price : " + price;
           




            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "nom,prix,quantite,description,Categorie")] Product product, HttpPostedFileBase Image)
        {
            product.userId = 1;
            product.imageString = "null";
            product.imageByte = new byte[Image.ContentLength];
            Image.InputStream.Read(product.imageByte, 0, Image.ContentLength);
            product.imageString = Convert.ToBase64String(product.imageByte);

            product.dateAjout = DateTime.UtcNow;
            serviceProd.Add(product);
            serviceProd.Commit();
            return RedirectToAction("Index");
        }

        //// GET: Product/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Product/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Product/Create
        public ActionResult DisplaySelectedProduct(int id)
        {

            Product product = serviceProd.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost]
        public ActionResult AddProductToMyCart()
        {
            int quantiteChoisie = Int16.Parse(Request["quantite"].ToString());
            int productId = Int16.Parse(Request["productId"].ToString());

            try
            {
                Cart cartForTest = serviceCart.GetCartByUserId(1);
                Product product = serviceProd.GetById(productId);

                if (cartForTest == null)
                {
                    Cart newCart = db.Carts.Create();
                    newCart.status = true;
                    newCart.userId = 1;
                    newCart.dateAchat = DateTime.UtcNow;
                    serviceCart.Add(newCart);
                    serviceCart.Commit();


                    CartLine cartLine = db.CartLines.Create();
                    cartLine.dateAjout = DateTime.UtcNow;
                    cartLine.CartId = newCart.id;
                    cartLine.productId = productId;
                    cartLine.quantiteChoisie = quantiteChoisie;
                    cartLine.prixTotal = product.prix * quantiteChoisie;
                    cartLine.prixDuProduit = product.prix;
                    serviceCartLine.Add(cartLine);
                    serviceCartLine.Commit();

                    //newCart.prixTotal += cartLine.prixTotal;
                    //serviceCart.Commit();

                }
                else
                {
                    CartLine cartLine = db.CartLines.Create();
                    cartLine.dateAjout = DateTime.UtcNow;
                    cartLine.CartId = cartForTest.id;
                    cartLine.productId = productId;
                    cartLine.quantiteChoisie = quantiteChoisie;
                    cartLine.prixTotal = product.prix * quantiteChoisie;
                    cartLine.prixDuProduit = product.prix;
                    serviceCartLine.Add(cartLine);
                    serviceCartLine.Commit();

                    //cartForTest.prixTotal += cartLine.prixTotal ;
                    //serviceCart.Commit();
                }
            }

            catch (IOException e)
            {
                throw e;
            }

            return RedirectToAction("Index");
        }


        // GET: Product/Create
        public ActionResult MyCart()
        {
            Cart cart = null;
            cart = serviceCart.GetCartByUserId(1);
            if (cart != null)
            {
                var cartLines = serviceCartLine.GetCartLinesByCartId(cart.id);

                if (cartLines.Count == 0)
                {
                    return View();
                }
                return View(cartLines);
            }
            return View();
        }


        // GET: Product/Create
        public ActionResult CartHistory()
        {
            List<Cart> carts = serviceCart.GetAllCartByUserId(1);

            if (carts.Count != 0)
            {

                return View(carts);
            }
            return View();
        }



        public ActionResult DeleteCartLine(int id)
        {

            CartLine cartLine = serviceCartLine.GetById(id);
            // serviceCartLine.DeleteCartLine(id);
            serviceCartLine.Delete(cartLine);
            serviceCartLine.Commit();
            return RedirectToAction("MyCart");
        }



        // ************************************************************* Paypal Payment **************************************************************//
        private Payment payment;

        //Create a payment using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            Cart myCart = serviceCart.GetCartByUserId(1);
            var cartLines = myCart.CartLines;

            var listItems = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var cartline in cartLines)
            {
                listItems.items.Add(new Item()
                {
                    name = cartline.myProduct.nom,
                    currency = "EUR",
                    price = cartline.myProduct.prix.ToString(),
                    quantity = cartline.quantiteChoisie.ToString(),
                    sku = "sku"
                });

            }
            var payer = new Payer() { payment_method = "paypal" };
            // Do the configuration RedirectURLs here with redirectURLs object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            // Create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = cartLines.Sum(x => x.quantiteChoisie * x.myProduct.prix).ToString()

            };

            //Create amount object
            var amount = new Amount()
            {
                currency = "EUR",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details

                // tax + shipping + subtotal
            };

            //Create transaction
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = " Testring transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }

        //Create Execute Payment method
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }


        public ActionResult ConfirmPurchase()
        {
            Cart myCart = serviceCart.GetCartByUserId(1);
            ICollection<CartLine> cartLines = serviceCartLine.GetCartLinesByCartId(myCart.id);
            double totalPrice = 0;
            foreach (var cartLine in cartLines)
            {
                totalPrice += (cartLine.prixDuProduit * cartLine.quantiteChoisie);
            }
            myCart.prixTotal = totalPrice;
            myCart.status = false;
            //myCart.CartLines = cartLines;
            SendVerificationLinkEmail("souhaib.roblehsouldan@esprit.tn", cartLines);
            serviceCart.Commit();


            //int points = Convert.ToInt32(totalPrice / 3);

            //MyProductService.CartConfirmPurchase(user.Id);
            return RedirectToAction("CartHistory");
        }


        public ActionResult EditCartLine(int id)
        {
            CartLine carLine = serviceCartLine.GetById(id);
            return View(carLine);
        }

        [HttpPost]
        public ActionResult EditCartLineDB()
        {
            int quantiteChoisie = Int16.Parse(Request["quantite"].ToString());
            int cartLineId = Int16.Parse(Request["cartLineId"].ToString());


            CartLine cartLine = serviceCartLine.GetById(cartLineId);
            cartLine.prixTotal = (cartLine.prixDuProduit * quantiteChoisie);
            cartLine.quantiteChoisie = quantiteChoisie;
            serviceCartLine.Update(cartLine);
            serviceCartLine.Commit();

            return RedirectToAction("MyCart");
        }


    }
}
