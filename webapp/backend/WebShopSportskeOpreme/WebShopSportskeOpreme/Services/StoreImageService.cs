using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Helpers;

namespace WebShopSportskeOpreme.Services
{
    public class StoreImageService : IStoreImageService
    {

        private readonly WebShopDbContext _context;
        private readonly IStoreService _storeService;

        public StoreImageService(WebShopDbContext context, IStoreService storeService)
        {
            _context = context;
            _storeService = storeService;
        }

        public bool CreateStoreImage(int storeId, string image)
        {
            if (_context.Stores.Where(x => x.Id == storeId) == null || String.IsNullOrEmpty(image))
                return false;

            int commaIndex = image.IndexOf(',');
            var format = image.Substring(0, commaIndex + 1);
            var imageString = image.Substring(commaIndex + 1);
            var img = new StoreImage()
            {
                StoreID = storeId,
                ImageByteArray = imageString.parseBase64(),
                ImageFormat = format
            };
            if (_context.StoreImages.Where(x => x.StoreID == storeId).Any())
            {
                var obj = _context.StoreImages.Where(x => x.StoreID == storeId).FirstOrDefault();
                _context.StoreImages.Remove(obj);
            }
            _context.StoreImages.Add(img);

            _context.SaveChanges();
            return true;
        }

        public bool DeleteStoreImageByStoreId(int storeId)
        {
            var img = _context.StoreImages.FirstOrDefault(x => x.StoreID == storeId);
            if (img == null)
                return false;
            _context.StoreImages.Remove(img);
            _context.SaveChanges();
            return true;
        }

        public List<StoreImage> GetAllStoreImages()
        {
            return _context.StoreImages.ToList();
        }

        public List<StoreImage> GetImagesByStoreId(int storeId)
        {
            return _context.StoreImages.Where(x => x.StoreID == storeId).ToList();
        }


        public List<StoreImage> GetStoreImagesByStoreName(string name)
        {
            var list = new List<StoreImage>();
            if (String.IsNullOrEmpty(name))
                return _context.StoreImages.ToList();
            var stores = _storeService.GetStoreByName(name);
            foreach (Store item in stores)
            {
                var storeImg = _context.StoreImages.Where(x => x.StoreID == item.Id).First();
                if (storeImg != null)
                    list.Add(storeImg);
            }
            return list.Distinct().ToList();
        }
    }
}
