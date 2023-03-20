using Model.EF;
using Model.Repository;

namespace Model
{
    public class WorkerClass
    {
        private OnlineShopDataContext data;
        public UserAccountRepository UserAccountRepo;
        public EmployeeAccountRepository EmployeeAccountRepo;
        public SlideRepository SlideRepo;
        public ContactRepository ContactRepo;
        public SupplierRepository SupplierRepo;
        public ProductRepository ProductRepo;
        public ProductCategoryRepository ProductCategoryRepo;
        public SizeRepository SizeRepo;
        public ColorRepository ColorRepo;
        public AgeRepository AgeRepo;
        public ProductPhotoRepository ProductPhotoRepo;
        public StockRepository StockRepo;
        public ProductOrderRepository ProductOrderRepo;
        public InvoiceRepository InvoiceRepo;
        public ProductOrderDetailRepository ProductOrderDetailRepo;
        public FavouriteRepository FavouriteRepo;
        public EvaluationRepository EvaluationRepo;
        public FeedbackRepository FeedbackRepo;
        public ShoppingCartRepository ShoppingCartRepo;
        public PaymentRepository PaymentRepo;
        #region Making Singleton Unit Of Work
        private static WorkerClass ins;
        public static WorkerClass Ins
        {
            get
            {
                if (ins == null)
                    ins = new WorkerClass();
                return ins;
            }
            set
            {
                ins = value;
            }
        }
        public WorkerClass()
        {
            data = new OnlineShopDataContext();
            //init database before init repo to remove error (The context cannot be used while the model is being created)
            data.Database.Initialize(force: false);
            UserAccountRepo = new UserAccountRepository(data);
            EmployeeAccountRepo = new EmployeeAccountRepository(data);
            SlideRepo = new SlideRepository(data);
            ContactRepo = new ContactRepository(data);
            SupplierRepo = new SupplierRepository(data);
            ProductRepo = new ProductRepository(data);
            ProductCategoryRepo = new ProductCategoryRepository(data);
            SizeRepo = new SizeRepository(data);
            ColorRepo = new ColorRepository(data);
            AgeRepo = new AgeRepository(data);
            ProductPhotoRepo = new ProductPhotoRepository(data);
            StockRepo = new StockRepository(data);
            ProductOrderRepo = new ProductOrderRepository(data);
            InvoiceRepo = new InvoiceRepository(data);
            ProductOrderDetailRepo = new ProductOrderDetailRepository(data);
            FavouriteRepo = new FavouriteRepository(data);
            EvaluationRepo = new EvaluationRepository(data);
            FeedbackRepo = new FeedbackRepository(data);
            ShoppingCartRepo = new ShoppingCartRepository(data);
            PaymentRepo = new PaymentRepository(data);
        }
        #endregion
        public void Save()
        {
            data.SaveChanges();
        }
    }
}
