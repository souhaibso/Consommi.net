using Solution.Data;
using Solution.Domain.Entities;
using Solution.Service;
using Solution.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace Solution.Web.Controllers.Api
{

    public class CartProductApiController : ApiController
    {
        IProductService serviceProduct;
        ICartLineService serviceCartLine;
        ICartService serviceCart;
        MyContext db = new MyContext();
        public CartProductApiController()
        {

            serviceProduct = new ProductService();
            serviceCartLine = new CartLineService();
            serviceCart = new CartService();
        }

        [Route("Api/GetAllProduct")]
        public IHttpActionResult GetAllProductWebService()
        {
            List<Product> productList = db.Products.ToList();
            List<ProductModel> productModels = new List<ProductModel>();
            foreach (var p in productList)
            {
                productModels.Add(new ProductModel()
                {
                    Categorie = p.Categorie,
                    dateAjout = p.dateAjout,
                    description = p.description,
                    id = p.id,
                    imageString = p.imageString,
                    nom = p.nom,
                    prix = p.prix,
                    quantite = p.quantite,
                    userId = p.userId

                });
            }
            return Ok(productModels);

        }

        [Route("Api/GetProduct")]
        public IHttpActionResult GetProductWebService(int idp)
        {
            List<Product> productList = db.Products.Where(p=>p.id==idp).ToList();
            List<ProductModel> productModels = new List<ProductModel>();
            foreach (var p in productList)
            {
                productModels.Add(new ProductModel()
                {
                    Categorie = p.Categorie,
                    dateAjout = p.dateAjout,
                    description = p.description,
                    id = p.id,
                    imageString = p.imageString,
                    nom = p.nom,
                    prix = p.prix,
                    quantite = p.quantite,
                    userId = p.userId

                });
            }
            return Ok(productModels);

        }


        [Route("Api/GetAllCartByUserId/{userId}")]
        public IHttpActionResult GetAllCartByUserIdWebService(int userId)
        {
            var cartList = serviceCart.GetAllCartByUserId(userId);
            List<CartModel> cartModels = new List<CartModel>(); ;
            List<CartLineModel> CartLineModels;
            foreach (var c in cartList)
            {
                CartLineModels = new List<CartLineModel>();
                foreach (var f in c.CartLines)
                {
                    CartLineModels.Add(new CartLineModel()
                    {
                        myProduct = new ProductModel()
                        {
                            userId = f.myProduct.userId,
                            quantite = f.myProduct.quantite,
                            prix = f.myProduct.prix,
                            nom = f.myProduct.nom,
                            imageString = f.myProduct.imageString,
                            Categorie = f.myProduct.Categorie,
                            dateAjout = f.myProduct.dateAjout,
                            description = f.myProduct.description,
                            id = f.myProduct.id,
                        },
                        CartId = f.CartId,
                        dateAjout = f.dateAjout,
                        id = f.id,
                        prixDuProduit = f.prixDuProduit,
                        prixTotal = f.prixTotal,
                        productId = f.productId,
                        quantiteChoisie = f.quantiteChoisie

                    });

                }
                cartModels.Add(new CartModel()
                {
                    CartLines = CartLineModels,


                    dateAchat = c.dateAchat,
                    id = c.id,
                    prixTotal = c.prixTotal,
                    status = c.status,
                    userId = c.userId
                });
            }
            return Ok(cartModels);

        }


        [Route("Api/DeleteCartById/{cartId}")]
        public IHttpActionResult DeleteCartById(int cartId)
        {
            Cart cart = serviceCart.GetById(cartId);
            serviceCart.Delete(cart);
            serviceCart.Commit();
            return Ok("delete");

        }
        [NonAction]
        public void SendVerificationLinkEmail(string email, ICollection<CartLine> p)
        {
            String nomProd = "";
            int quantity = 0;
            double price = 0;

            foreach (var c in p)
            {
                nomProd = c.myProduct.nom;
                quantity += c.myProduct.quantite;
                price += c.prixTotal;

            };

            var fromEmail = new MailAddress("hsine.gabsi@esprit.tn", "Command Delivery");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Brigade2001";
            string subject = "Command Delivery";
            string body = "<br/><br>We are excited to tell you that your command " +
                             "will arrive in " + DateTime.Now.ToString("dd-MM-yyy") +
                             "<br/> Product Name : " + nomProd +
                             "<br/> Product Quantity : " + quantity +
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


        [HttpPost]
        [Route("Api/Create")]
        public IHttpActionResult Create(ProductModel productModel)
        {
            Product product = new Product();
            product.userId = 1;

            product.imageString = productModel.imageString;

            product.nom = productModel.nom;
            product.prix = productModel.prix;
            product.quantite = productModel.quantite;
            product.description = productModel.description;
            product.Categorie = productModel.Categorie;

            product.dateAjout = DateTime.UtcNow;

            serviceProduct.Add(product);
            serviceProduct.Commit();
            return Ok();
        }
        [HttpPost]
        [Route("Api/AddProductToMyCart")]
        public IHttpActionResult AddProductToMyCart(int idp, ProductModel p)
        {


            Cart cartForTest = serviceCart.GetCartByUserId(1);
            Product product = serviceProduct.GetById(idp);

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
                cartLine.productId = idp;
                cartLine.quantiteChoisie = p.quantite;
                cartLine.prixTotal = product.prix * p.quantite;
                cartLine.prixDuProduit = product.prix;
                serviceCartLine.Add(cartLine);
                serviceCartLine.Commit();



            }
            else
            {
                CartLine cartLine = db.CartLines.Create();
                cartLine.dateAjout = DateTime.UtcNow;
                cartLine.CartId = cartForTest.id;
                cartLine.productId = idp;
                cartLine.quantiteChoisie = p.quantite;
                cartLine.prixTotal = product.prix * p.quantite;
                cartLine.prixDuProduit = product.prix;
                serviceCartLine.Add(cartLine);
                serviceCartLine.Commit();

                //cartForTest.prixTotal += cartLine.prixTotal ;
                //serviceCart.Commit();
            }


            return Ok();
        }
        [HttpGet]
        [Route("Api/MyCart")]

        public IEnumerable<CartLineModel> MyCart()
        {

            Cart cart = serviceCart.GetCartByUserId(1);
            try
            {
                var cartLines = serviceCartLine.GetCartLinesByCartId(cart.id);
                List<CartLineModel> cartLineModels = new List<CartLineModel>();
                foreach (var c in cartLines)
                {
                    cartLineModels.Add(new CartLineModel()
                    {
                        CartId = c.CartId,
                        dateAjout = c.dateAjout,
                        id = c.id,
                        myProduct = new ProductModel()
                        {
                            userId = c.myProduct.userId,
                            quantite = c.myProduct.quantite,
                            prix = c.myProduct.prix,
                            nom = c.myProduct.nom,
                            imageString = c.myProduct.imageString,
                            Categorie = c.myProduct.Categorie,
                            dateAjout = c.myProduct.dateAjout,
                            description = c.myProduct.description,
                            id = c.myProduct.id,
                        },
                        prixDuProduit = c.prixDuProduit,
                        prixTotal = c.prixTotal,
                        productId = c.productId,
                        quantiteChoisie = c.quantiteChoisie,

                    });
                }

                return cartLineModels;

            }
            catch { return new List<CartLineModel>(); }


        }
        [HttpGet]
        [Route("Api/ConfirmPurchase")]
        public IHttpActionResult ConfirmPurchase()
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
            SendVerificationLinkEmail("souhaib.roblehsouldan@esprit.tn", cartLines);

            //myCart.CartLines = cartLines;
            serviceCart.Commit();

            //int points = Convert.ToInt32(totalPrice / 3);

            //MyProductService.CartConfirmPurchase(user.Id);
            return Ok();
        }
        [HttpPut]
        [Route("Api/EditCartLine")]
        public IHttpActionResult EditCartLineDB(int idc, CartLineModel p)
        {



            CartLine cartLine = serviceCartLine.GetById(idc);
            cartLine.prixTotal = (cartLine.prixDuProduit * p.quantiteChoisie);
            cartLine.quantiteChoisie = p.quantiteChoisie;
            serviceCartLine.Update(cartLine);
            serviceCartLine.Commit();

            return Ok();
        }


    }
}
