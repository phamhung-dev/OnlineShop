using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        public List<ShoppingCart> GetShoppingCart()
        {
            List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<ShoppingCart>();
                Session["ShoppingCart"] = lstShoppingCart;
            }
            return lstShoppingCart;
        }
        // GET: ShoppingCart
        [HttpPost]
        [ActionName("them-vao-gio-hang")]
        public ActionResult AddToShoppingCart(string productid, string sizeid, string colorid, string quantity)
        {
            try
            {
                int quantityInt = int.Parse(quantity);
                if(quantityInt <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn số lượng mua tối thiểu là 1 sản phẩm." }, JsonRequestBehavior.AllowGet);
                }
                var product = WorkerClass.Ins.StockRepo.GetProductInStockById(productid, sizeid, colorid);
                if(product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm này đã hết hàng." }, JsonRequestBehavior.AllowGet);
                }
                int quantityInStock = product.Quantity;
                if (quantityInStock < quantityInt)
                {
                    return Json(new { success = false, message = "Cửa hàng chỉ còn " + quantityInStock.ToString() + " sản phẩm." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<ShoppingCart> lstShoppingCart = GetShoppingCart();
                    ShoppingCart shoppingCartTemp = lstShoppingCart.Find(n => n.ProductID == productid && n.SizeID == sizeid && n.ColorID == colorid);
                    if(shoppingCartTemp == null)
                    {
                        ShoppingCart shoppingCart = new ShoppingCart() { ProductID = productid, SizeID = sizeid, ColorID = colorid, Quantity = quantityInt, Stock = product };
                        lstShoppingCart.Add(shoppingCart);
                        if(Session["CustomerCurrent"] != null)
                        {
                            shoppingCart.UserID = (Session["CustomerCurrent"] as UserAccount).UserID;
                            if (WorkerClass.Ins.ShoppingCartRepo.InsertShoppingCart(shoppingCart))
                            {
                                WorkerClass.Ins.Save();
                            }
                            else
                            {
                                return Json(new { success = false, message = "Thêm sản phẩm vào giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        Session["ShoppingCart"] = lstShoppingCart;
                        return Json(new { success = true, totalQuantityProductCategory = TotalQuantityProductCategory() }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Sản phẩm đã có trong giỏ hàng." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm sản phẩm vào giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        private int TotalQuantityProductCategory()
        {
            int totalQuantityShoes = 0;
            List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
            if (lstShoppingCart != null)
            {
                totalQuantityShoes = lstShoppingCart.Count;
            }
            return totalQuantityShoes;
        }
        public ActionResult ShoppingCartPartial()
        {
            ViewBag.TotalQuantityProductCategory = TotalQuantityProductCategory();
            return PartialView(ViewBag.TotalQuantityProductCategory);
        }
        [HttpPost]
        [ActionName("xoa-san-pham")]
        public ActionResult RemoveProductInShoppingCart(string productid, string sizeid, string colorid)
        {
            try
            {
                List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
                var product = lstShoppingCart.Find(n => n.ProductID == productid && n.SizeID == sizeid && n.ColorID == colorid);
                if (product == null)
                {
                    return Json(new { success = false, message = "Xóa thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    lstShoppingCart.Remove(product);
                    if (Session["CustomerCurrent"] != null)
                    {
                        if (WorkerClass.Ins.ShoppingCartRepo.DeleteShoppingCart((Session["CustomerCurrent"] as UserAccount).UserID, productid, sizeid, colorid))
                        {
                            WorkerClass.Ins.Save();
                        }
                        else
                        {
                            return Json(new { success = false, message = "Xóa thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    Session["ShoppingCart"] = lstShoppingCart;
                    return Json(new { success = true, totalQuantityProductCategory = TotalQuantityProductCategory(), totalPayment = lstShoppingCart.Sum(x => ((double)x.Stock.Product.UnitSellPrice) * (1 - x.Stock.Product.DiscountPercent / 100) * x.Quantity) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-gio-hang")]
        public ActionResult UpdateShoppingCart(string productid, string sizeid, string colorid, string quantity)
        {
            try
            {
                List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
                var product = lstShoppingCart.FirstOrDefault(n => n.ProductID == productid && n.SizeID == sizeid && n.ColorID == colorid);
                if (product == null)
                {
                    return Json(new { success = false, message = "Cập nhật giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int quantityInt = int.Parse(quantity);
                    product.Quantity = quantityInt;
                    if (Session["CustomerCurrent"] != null)
                    {
                        if (WorkerClass.Ins.ShoppingCartRepo.UpdateShoppingCart((Session["CustomerCurrent"] as UserAccount).UserID, productid, sizeid, colorid, quantityInt))
                        {
                            WorkerClass.Ins.Save();
                        }
                        else
                        {
                            return Json(new { success = false, message = "Cập nhật giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    Session["ShoppingCart"] = lstShoppingCart;
                    return Json(new { success = true, totalPayment = lstShoppingCart.Sum(x => ((double)x.Stock.Product.UnitSellPrice) * (1 - x.Stock.Product.DiscountPercent / 100) * x.Quantity), payment = ((double)product.Stock.Product.UnitSellPrice) * (1 - product.Stock.Product.DiscountPercent / 100) * product.Quantity }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("thanh-toan-don-hang")]
        public ActionResult Order()
        {
            if(Session["CustomerCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "UserAccount");
            }
            Guid productOrderID = Guid.NewGuid();
            while (WorkerClass.Ins.ProductOrderRepo.CheckProductOrderIdExist(productOrderID))
            {
                productOrderID = Guid.NewGuid();
            }
            ViewBag.ProductOrderID = productOrderID;
            ViewBag.ListPayment = WorkerClass.Ins.PaymentRepo.GetAllPayment();
            return View("Order",Session["ShoppingCart"] as List<ShoppingCart>);
        }
        [HttpPost]
        [ActionName("thanh-toan-khi-nhan-hang")]
        public ActionResult PaymentCOD(string productorderid)
        {
            try
            {
                ViewBag.Payment = "COD";
                if((Session["CustomerCurrent"] as UserAccount).EmailConfirmed == false)
                {
                    return Json(new { success = false, message = "Vui lòng thực hiện xác nhận email cho tài khoản này để có thể đặt hàng." }, JsonRequestBehavior.AllowGet);
                }
                List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
                Guid productOrderID = new Guid(productorderid);
                ProductOrder productOrder = new ProductOrder() { ProductOrderID = productOrderID, OrderDate = DateTime.Now, UserID = (Session["CustomerCurrent"] as UserAccount).UserID, PaymentID = "COD", PaymentOnlineID = null, Status = false };
                if (WorkerClass.Ins.ProductOrderRepo.InsertProductOrder(productOrder))
                {
                    foreach(var item in lstShoppingCart)
                    {
                        ProductOrderDetail productOrderDetail = new ProductOrderDetail() { ProductOrderID = productOrder.ProductOrderID, ProductID = item.ProductID, SizeID = item.SizeID, ColorID = item.ColorID, Price = (decimal)(((double)item.Stock.Product.UnitSellPrice) * (1 - item.Stock.Product.DiscountPercent / 100)), Quantity = item.Quantity };
                        if (!WorkerClass.Ins.ProductOrderDetailRepo.InsertProductOrderDetail(productOrderDetail))
                        {
                            return Json(new { success = false, message = "Thanh toán thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (Session["CustomerCurrent"] != null)
                    {
                        if (WorkerClass.Ins.ShoppingCartRepo.DeleteAllShoppingCart((Session["CustomerCurrent"] as UserAccount).UserID))
                        {
                            WorkerClass.Ins.Save();
                        }
                        else
                        {
                            return Json(new { success = false, message = "Thanh toán thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    Session["ShoppingCart"] = null;
                    return Json(new { success = true, url = "/gio-hang/ket-qua-dat-hang" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thanh toán thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thanh toán thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("thanh-toan-truc-tuyen")]
        public ActionResult PaymentOnline(string productorderid)
        {
            try
            {
                if ((Session["CustomerCurrent"] as UserAccount).EmailConfirmed == false)
                {
                    return Json(new { success = false, message = "Vui lòng thực hiện xác nhận email cho tài khoản này để có thể đặt hàng." }, JsonRequestBehavior.AllowGet);
                }
                List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
                var totalPayment = lstShoppingCart.Sum(x => ((double)x.Stock.Product.UnitSellPrice) * (1 - x.Stock.Product.DiscountPercent / 100) * x.Quantity);
                string url = ConfigurationManager.AppSettings["Url"].ToString();
                string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"].ToString();
                string tmnCode = ConfigurationManager.AppSettings["TmnCode"].ToString();
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"].ToString();

                VnPayLibrary pay = new VnPayLibrary();

                pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
                pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                pay.AddRequestData("vnp_Amount", (totalPayment * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
                pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                pay.AddRequestData("vnp_OrderInfo", "Thanh toan ma don hang: " + productorderid); //Thông tin mô tả nội dung thanh toán
                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                Session["ProductOrderID"] = productorderid;

                return Json(new { success = true, paymentUrl = paymentUrl }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Thanh toán thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("ket-qua-thanh-toan-truc-tuyen")]
        public ActionResult PaymentOnlineResult()
        {
            NeighborhoodBasedCollaborativeFiltering nbcf = new NeighborhoodBasedCollaborativeFiltering();
            var ListProductRecommend = nbcf.GetListProductRecommend((Session["CustomerCurrent"] as UserAccount).UserID).OrderByDescending(x => x.ScoreRatingPrediction).ToList();
            var ListProductSelling = WorkerClass.Ins.ProductRepo.GetAllProductSelling();
            var ListProductOrderDetailBought = WorkerClass.Ins.ProductOrderDetailRepo.GetListProductOrderDetailBought((Session["CustomerCurrent"] as UserAccount).UserID);
            List<Product> ListProductRecommendFinish = new List<Product>();
            foreach (var item in ListProductRecommend)
            {
                if (ListProductOrderDetailBought.FirstOrDefault(x => x.ProductID == item.ProductID) == null)
                {
                    var product = ListProductSelling.FirstOrDefault(x => x.ProductID == item.ProductID);
                    ListProductRecommendFinish.Add(product);
                }
            }
            ListProductRecommendFinish = ListProductRecommendFinish.Take(4).ToList();
            try
            {
                ViewBag.Payment = "ONLINE";
                if(Session["ProductOrderID"] == null)
                {
                    return RedirectToAction("trang-chu", "Main");
                }
                Guid productOrderID = new Guid(Session["ProductOrderID"].ToString());
                if (Request.QueryString.Count > 0)
                {
                    string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                    var vnpayData = Request.QueryString;
                    VnPayLibrary pay = new VnPayLibrary();

                    //lấy toàn bộ dữ liệu được trả về
                    foreach (string s in vnpayData)
                    {
                        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                        {
                            pay.AddResponseData(s, vnpayData[s]);
                        }
                    }

                    long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                    long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                    string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                    string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                    bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00")
                        {
                            //Thanh toán thành công
                            List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
                            ProductOrder productOrder = new ProductOrder() { ProductOrderID = productOrderID, OrderDate = DateTime.Now, UserID = (Session["CustomerCurrent"] as UserAccount).UserID, PaymentID = "ONLINE", PaymentOnlineID = vnpayTranId.ToString(), Status = false };
                            WorkerClass.Ins.ProductOrderRepo.InsertProductOrder(productOrder);
                            
                            foreach (var item in lstShoppingCart)
                            {
                                ProductOrderDetail productOrderDetail = new ProductOrderDetail() { ProductOrderID = productOrder.ProductOrderID, ProductID = item.ProductID, SizeID = item.SizeID, ColorID = item.ColorID, Price = (decimal)(((double)item.Stock.Product.UnitSellPrice) * (1 - item.Stock.Product.DiscountPercent / 100)), Quantity = item.Quantity };
                                WorkerClass.Ins.ProductOrderDetailRepo.InsertProductOrderDetail(productOrderDetail);
                            }
                            if (Session["CustomerCurrent"] != null)
                            {
                                WorkerClass.Ins.ShoppingCartRepo.DeleteAllShoppingCart((Session["CustomerCurrent"] as UserAccount).UserID);
                                WorkerClass.Ins.Save();
                            }
                            Session["ShoppingCart"] = null;
                            Session["ProductOrderID"] = null;
                            ViewBag.IsPaymentSuccess = true;
                            ViewBag.PaymentResult = "Thông báo từ VNPAY: Thanh toán thành công phiếu đặt hàng " + productOrderID.ToString() + " | Mã giao dịch: " + vnpayTranId + ".";
                            return View("OrderResult", ListProductRecommendFinish);
                        }
                        else
                        {
                            //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                            ViewBag.IsPaymentSuccess = false;
                            ViewBag.PaymentResult = "Thông báo từ VNPAY: Có lỗi xảy ra trong quá trình xử lý phiếu đặt hàng " + productOrderID.ToString() + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode + ".";
                            return View("OrderResult", ListProductRecommendFinish);
                        }
                    }
                    else
                    {
                        ViewBag.IsPaymentSuccess = false;
                        ViewBag.PaymentResult = "Thông báo từ VNPAY: Có lỗi xảy ra trong quá trình xử lý phiếu đặt hàng " + productOrderID.ToString() + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode + ".";
                        return View("OrderResult", ListProductRecommendFinish);
                    }
                }
                else
                {
                    ViewBag.IsPaymentSuccess = false;
                    ViewBag.PaymentResult = "Thông báo từ VNPAY: Có lỗi xảy ra trong quá trình xử lý phiếu đặt hàng " + productOrderID.ToString() + ".";
                    return View("OrderResult", ListProductRecommendFinish);
                }
            }
            catch
            {
                ViewBag.IsPaymentSuccess = false;
                ViewBag.PaymentResult = "Thông báo từ VNPAY: Có lỗi xảy ra trong quá trình xử lý phiếu đặt hàng.";
                return View("OrderResult", ListProductRecommendFinish);
            }
        }
        [ActionName("ket-qua-dat-hang")]
        public ActionResult OrderResult()
        {
            if (Session["CustomerCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "UserAccount");
            }
            NeighborhoodBasedCollaborativeFiltering nbcf = new NeighborhoodBasedCollaborativeFiltering();
            var ListProductRecommend = nbcf.GetListProductRecommend((Session["CustomerCurrent"] as UserAccount).UserID).OrderByDescending(x => x.ScoreRatingPrediction).ToList();
            var ListProductSelling = WorkerClass.Ins.ProductRepo.GetAllProductSelling();
            var ListProductOrderDetailBought = WorkerClass.Ins.ProductOrderDetailRepo.GetListProductOrderDetailBought((Session["CustomerCurrent"] as UserAccount).UserID);
            List<Product> ListProductRecommendFinish = new List<Product>();
            foreach (var item in ListProductRecommend)
            {
                if(ListProductOrderDetailBought.FirstOrDefault(x => x.ProductID == item.ProductID) == null)
                {
                    var product = ListProductSelling.FirstOrDefault(x => x.ProductID == item.ProductID);
                    ListProductRecommendFinish.Add(product);
                }
            }
            ListProductRecommendFinish = ListProductRecommendFinish.Take(4).ToList();
            return View("OrderResult", ListProductRecommendFinish);
        }
    }
}