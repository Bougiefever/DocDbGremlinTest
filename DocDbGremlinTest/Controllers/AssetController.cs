using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocDbGremlinTest.Data;
using DocDbGremlinTest.Models;

namespace DocDbGremlinTest.Controllers
{
    public class AssetController : Controller
    {
        private ItemRepository _itemRepo;
        private BuildingDocRepository _buildingDocRepo;
        private FloorDocRepository _floorDocRepo;
        private RoomDocRepository _roomDocRepo;
        private FurnitureDocRepository _furnitureDocRepository;
        private ElectronicDocRepository _electronicDocRepository;
        private ItemGraphRepository _itemGraphRepository;

        // GET: Asset
        public AssetController()
        {
            _itemRepo = new ItemRepository();
            _buildingDocRepo = new BuildingDocRepository();
            _floorDocRepo = new FloorDocRepository();
            _roomDocRepo = new RoomDocRepository();
            _furnitureDocRepository = new FurnitureDocRepository();
            _electronicDocRepository = new ElectronicDocRepository();
            _itemGraphRepository = new ItemGraphRepository();
            //_itemGraphRepository.Setup().Wait();
        }

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var it = _itemGraphRepository.Test("building1");
            IEnumerable<Item> items = await _itemRepo.GetItemsAsync(x => true);
            return View(items);
        }

        // GET: Asset/Details/5
        public async Task<ActionResult> CreateBuilding()
        {
            var building = new Building { AccountId = 1, ItemType = ItemType.Building };
            return View(building);
        }

        [HttpPost]
        [ActionName("CreateBuilding")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBuilding(Building building)
        {
            if (ModelState.IsValid)
            {
                //await _itemGraphRepository.Setup();
                await _buildingDocRepo.CreateItemAsync(building);
                await _itemGraphRepository.AddItem(building);
                return RedirectToAction("Index");
            }

            return View(building);
        }

        public async Task<ActionResult> CreateFloor(string id)
        {
            var buildingId = id;
            Session["buildingid"] = buildingId;
            var floor = new Floor { AccountId = 1, ItemType = ItemType.Floor };
            return View(floor);
        }

        [HttpPost]
        [ActionName("CreateFloor")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFloor(Floor floor)
        {
            if (ModelState.IsValid)
            {
                string buildingId = (string)Session["buildingid"];
                await _floorDocRepo.CreateItemAsync(floor);
                await _itemGraphRepository.AddItem(floor);
                var building = await _buildingDocRepo.GetItemAsync(buildingId);
                await _itemGraphRepository.AddRelationship(building.Id, floor.Id, "has");
                return RedirectToAction("Index");
            }

            return View(floor);
        }

        public async Task<ActionResult> EditBuilding(string id)
        {
            var building = await _buildingDocRepo.GetItemAsync(id);
            return View(building);
        }

        [HttpPost]
        [ActionName("EditBuilding")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBuilding(Building building)
        {
            if (ModelState.IsValid)
            {
                await _buildingDocRepo.UpdateItemAsync(building.Id, building);
                return RedirectToAction("Index");
            }

            return View(building);
        }

        public async Task<ActionResult> BuildingDetails(string id)
        {
            var building = await _buildingDocRepo.GetItemAsync(id);
            var floorIds = await _itemGraphRepository.GetRelated(building.Id, "has", "floor");
            var floors = await _floorDocRepo.GetItemsAsync(floorIds);
            var buildingView = new BuildingView { Building = building, Floors = floors };
            //var result = await _itemGraphRepository.GetAssetsInBuilding(id);
            return View(buildingView);
        }

        public async Task<ActionResult> EditFloor(string id)
        {
            var floor = await _floorDocRepo.GetItemAsync(id);
            return View(floor);
        }

        [HttpPost]
        [ActionName("EditFloor")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFloor(Floor floor)
        {
            if (ModelState.IsValid)
            {
                await _floorDocRepo.UpdateItemAsync(floor.Id, floor);
                return RedirectToAction("Index");
            }

            return View(floor);
        }

        public ActionResult ListFloors()
        {
            IEnumerable<Floor> floors = new List<Floor>();
            return View(floors);
        }

        public ActionResult ListRooms()
        {
            IEnumerable<Room> rooms = new List<Room>();
            return View(rooms);
        }

        public ActionResult CreateRoom(string id)
        {
            Session["floorid"] = id;
            var room = new Room { AccountId = 1, ItemType = ItemType.Room };
            return View(room);
        }

        [HttpPost]
        [ActionName("CreateRoom")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomDocRepo.CreateItemAsync(room);
                await _itemGraphRepository.AddItem(room);
                var floor = await _floorDocRepo.GetItemAsync((string)Session["floorid"]);
                await _itemGraphRepository.AddRelationship(floor.Id, room.Id, "has");
                return RedirectToAction("Index");
            }

            return View(room);
        }

        public async Task<ActionResult> EditRoom(string id)
        {
            var room = await _roomDocRepo.GetItemAsync(id);
            return View(room);
        }

        public async Task<ActionResult> FloorDetails(string id)
        {
            var floor = await _floorDocRepo.GetItemAsync(id);
            var roomIds = await _itemGraphRepository.GetRelated(floor.Id, "has", "room");
            var rooms = await _roomDocRepo.GetItemsAsync(roomIds);
            var floorView = new FloorView() { Floor = floor, Rooms = rooms };
            return View(floorView);
        }

        public async Task<ActionResult> RoomDetails(string id)
        {
            var room = await _roomDocRepo.GetItemAsync(id);

            return View(new RoomView { Room = room });
        }

        public ActionResult CreateFurniture(string id)
        {
            Session["roomid"] = id;
            var furniture = new FurnitureItem { AccountId = 1, ItemType = ItemType.Furniture };
            return View(furniture);
        }

        [HttpPost]
        [ActionName("CreateFurniture")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFurniture(FurnitureItem furniture)
        {
            if (ModelState.IsValid)
            {
                await _furnitureDocRepository.CreateItemAsync(furniture);
                await _itemGraphRepository.AddItem(furniture);
                var room = await _roomDocRepo.GetItemAsync((string)Session["roomid"]);
                await _itemGraphRepository.AddRelationship(room.Id, furniture.Id, "has");
                return RedirectToAction("Index");
            }

            return View(furniture);
        }

        public ActionResult CreateElectronic(string id)
        {
            Session["roomid"] = id;
            var electronic = new ElectronicItem { AccountId = 1, ItemType = ItemType.ElectronicDevice };
            return View(electronic);
        }

        [HttpPost]
        [ActionName("CreateElectronic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateElectronic(ElectronicItem electronic)
        {
            if (ModelState.IsValid)
            {
                await _electronicDocRepository.CreateItemAsync(electronic);
                await _itemGraphRepository.AddItem(electronic);
                var room = await _roomDocRepo.GetItemAsync((string)Session["roomid"]);
                await _itemGraphRepository.AddRelationship(room.Id, electronic.Id, "has");
                return RedirectToAction("Index");
            }

            return View(electronic);
        }

        public async Task<ActionResult> EditFurniture(string id)
        {
            var furniture = await _furnitureDocRepository.GetItemAsync(id);
            return View(furniture);
        }

        [HttpPost]
        public async Task<ActionResult> MoveFurniture(MoveRoom move)
        {
            string id = (await _itemGraphRepository.GetRelated(move.Id, "has", "room")).First();
            await _itemGraphRepository.RemoveRelationship(move.Id, id, "has");
            await _itemGraphRepository.AddRelationship(move.Id, move.NewRoom, "has");
            return RedirectToAction("Index");
        }

    }

    public class MoveRoom
    {
        public string Id { get; set; }
        public string NewRoom { get; set; }
    }
}
