using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Vechile_Tracking_Systems.Models;

namespace Vechile_Tracking_Systems.Controllers
{
    public class VehicleManagementController : Controller
    {

        // GET: VehicleManagement

        DFLDBEntiti db = new DFLDBEntiti();
        DetailInfo detail = new DetailInfo();
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult MainGateIn()
        {
            List<string> products = new List<string>();
            List<string> uom = new List<string>();
            List<string> shipmentDocNumbers = new List<string>();


            foreach (var item in db.SP_SelectProductList())
            {
                products.Add(item.ToString());

            }
            ViewBag.Checking1 = "OK";
            ViewBag.ProductsList = products;

            foreach (var item in db.SP_SelectUOMList())
            {
                uom.Add(item.ToString());
            }
            ViewBag.Checking2 = "OK";
            ViewBag.UOMList = uom;
            long counter = 1;
            foreach (var item in db.SP_VehicleQRCode())
            {
                ViewBag.Checking3 = "OK";
                counter += item.TranID;
                ViewBag.TransQrCode = counter;
                Session["QR"] = counter;
            }

            foreach (var item in db.SP_ShipmentDocNo())
            {
                shipmentDocNumbers.Add(item.ToString());
            }
            ViewBag.Checking4 = "OK";
            ViewBag.ShipmentDocNoList = shipmentDocNumbers.ToArray();


            
            return View();
        }
        

        [HttpPost]
        public ActionResult MainGateIn(
            long tranId, DateTime transacationDate, string tranType, string vehicleEntryMode,
            long vehicleNatureId, long vehicleId, string vechicleNo, long transporterId, long DriverId,
            string NIC, long rfCardNo, string partyType, string partyDescription, string partyPONo, DateTime partyPODate,
            DateTime docDate, string entryStatus, string shipmentDocumentNo, DateTime shipmentDocumentDate,
            string docNo, string shipmentRemarks1, string shipmentRemarks2)
        {
            db.SP_Insert(
            tranId, transacationDate, tranType, vehicleEntryMode, vehicleNatureId, vehicleId, vechicleNo, transporterId, DriverId, NIC,
            rfCardNo, partyType, partyDescription, partyPONo, partyPODate, docNo, docDate, entryStatus,
            shipmentDocumentNo, shipmentDocumentDate, shipmentRemarks1, shipmentRemarks2
            );

            return View();
        }
        
        [HttpGet]
        
        public ActionResult InOutBound(string ProcessType)
        {
            List<object> deviceList = new List<object>();
            List<object> partyInbound = new List<object>();
            List<object> partyOutbound = new List<object>();
           
            foreach (var item in db.SP_InBound())
                {
                //item.Supplier_Description;
                partyInbound.Add(item);

                }
            
                foreach (var item in db.SP_OutBound())
                {
                //item.Customer_Description;
                partyOutbound.Add(item);
                }
           
            foreach (var item in db.SP_DeviceLocate(Session["userName"].ToString(), ProcessType))
            {
                deviceList.Add(item);
            }
            if (ProcessType == "Inbound")
            {
                TempData["res"] = ProcessType;
                TempData["type"] = "DO";
                TempData["BoundList"] = partyInbound.ToArray();
            }
            else
            {
                TempData["res"] = ProcessType;
                TempData["type"] = "DO";
                TempData["BoundList"] = partyOutbound.ToArray();
            }
            TempData["DeviceLocation"] = deviceList.ToArray();
            return RedirectToAction("MainGateIn");
        }
        public JsonResult WeightInfo()
        {
            List<WeightInfo> weight = new List<WeightInfo>();
            WeightInfo obj = new WeightInfo();

            foreach (var item in db.Sp_VehichleInfo())
            {
                obj.VehicleNo = item.VehicleNo;
                obj.rfCardId = (long)item.RFCardID;
                weight.Add(obj);
            }

            return Json(weight.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult VehicleDetails(string vechicleNo)
        {
            List<VehicleModel> vehicleList = new List<VehicleModel>();
            VehicleModel vehicle = new VehicleModel();

            var data = db.SP_VehicleInMainGateINOUT(vechicleNo);
            foreach (var item in data)
            {
                if (item.VehicleTitle == vechicleNo)
                {
                    vehicle.VehicleTitle = item.VehicleTitle;
                    vehicle.TransportTitle = item.TransporterTitle;
                    vehicle.DriverTitle = item.DriverTitle;
                    vehicle.NIC = item.NIC;
                    vehicle.VehicleNature = item.VehicleNatureTitle;
                    
                }
                vehicleList.Add(vehicle);
            }
            return Json(vehicleList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddNewVehicleEntries(long vehicleCode , string vehicleTitle, long vehicleTypeId, long vehicleNatureId, long vehicleCategoryId, double minWeightPayable, double length, long transporterId,
             double height, double width, int wheels, double capacity, int axle, int mfgYearId, int priority, double minLoad, double maxLoad, int preferenceNo, string tranType , string vNumber)
        {
            db.SP_INSERT_New_Vehicles(vehicleCode, "", "", vehicleTitle, 1, vehicleNatureId, vehicleTypeId, minWeightPayable, vehicleCategoryId, transporterId, length,
            width, height, capacity, wheels, axle, mfgYearId, priority, preferenceNo, minLoad, maxLoad, 1, tranType, vNumber);
            return RedirectToAction("DataEntry", "VehicleManagement");
        }
        public ActionResult Loading()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loading(string qrNo , string loading)
        {
            
            db.SP_InsertActivityLog(qrNo, loading);
            return RedirectToAction("MainGateIn");
        }
        public ActionResult Weighbrige()
        {
            
            List<string> list1 = new List<string>();
            List<long> list2 = new List<long>();
            ;
            foreach (var item in db.Sp_VehichleInfo())
            {
                list1.Add(item.VehicleNo);
                list2.Add((long)item.RFCardID);
            }
            ViewBag.VehicleNo = list1;
            ViewBag.rfCardId = list2;
            return View();

        }

        

        public ActionResult GetWeight()
        {

            SerialPort sp = new SerialPort();
            sp.PortName = "COM1";
            sp.BaudRate = 1200;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.RtsEnable = true;
            sp.DtrEnable = true;
            sp.ReadTimeout = 4000;

            if (!(sp.IsOpen))
            {
                sp.Open();
            }

            string data = sp.ReadExisting().ToString();

            Regex digits = new Regex(@"^\D*?((-?(\d+(\.\d+)?))|(-?\.\d+)).*");
            Match mx = digits.Match(data);

            decimal strValue = mx.Success ? Convert.ToDecimal(mx.Groups[1].Value) : 0;
            int VALUE = 0;
            if (strValue > 1000)
            {
                VALUE = (int)strValue / 100;

            }
            else if (strValue < 1000)
            {
                VALUE = (int)strValue / 10;

            }
            ViewBag.VehicleWeight = VALUE.ToString() + " KG";
            //db.SP_InsertWeighmentEntries()
            return RedirectToAction("Weighbrige", "VehicleManagement");
        }

        public ActionResult Delivery()
        {
            return View();
        }
        public ActionResult CreateQRCode()
        {
            Vechile_Tracking_Systems.CrystalReport.POS obj = new CrystalReport.POS();
            obj.SetDatabaseLogon("sa", "Dalda123");
            obj.PrintOptions.PrinterName = "BlackCopper 80mm Series";
            obj.PrintToPrinter(1, false, 0, 0);
            return RedirectToAction("MainGateIn");
        }
        [HttpGet]
        public ActionResult DataEntry()
        {
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<int> list4 = new List<int>();
            List<object> dirivers = new List<object>();
            long id = 1;
            long count = 1;
            foreach (var item in db.Sp_VehichleDetail())
            {
                id = item.VehicleID;
                
            }
            count += id;
            ViewBag.iD = count;
            foreach (var item in db.SP_TransporterList())
            {
                list1.Add(item.ToString());
            }
            ViewBag.Check1 = "OK";
            ViewBag.TranporterList = list1;

            foreach (var item in db.SP_VehicleNatureType())
            {
               list2.Add(item.VehicleNatureTitle);
               list3.Add(item.VehicleNatureType);
            }
            ViewBag.Check2 = "OK";
            ViewBag.VehicleNatureTitleList = list2;
            ViewBag.Check3 = "OK";
            ViewBag.VehicleNatureTypeList = list3;
            foreach (var item in db.SP_MfgYearList())
            {
                list4.Add((int)item.Value);
            }
            ViewBag.Check4 = "OK";
            ViewBag.VehicleMfgYearList = list4;

            foreach (var item in db.SP_DriverList())
            {
                dirivers.Add(item);
            }
            ViewBag.Check5 = "OK";
            ViewBag.DriverList = dirivers.ToArray();

            return View();
        }

        [HttpPost]
        public ActionResult DataEntry(string transpCode, string transptitle, string transptype, int totalVehicle, string textA, string textB, string telephone, string fax, string Em, string Web, int preNo
                                     , string DrivCod, string DrivTitle, long transporter, string Cnic, string phoneNo, string moboNo)
        {

            db.Sp_InsertTransport(transpCode, "", transptitle, transptype, totalVehicle, textA, textB, telephone, fax, Em, Web, preNo);

            db.SP_InsertDetailsDriver(DrivCod, DrivTitle, transporter, Cnic, phoneNo, moboNo);

            db.SaveChanges();
            
            return View();
        }



        [HttpPost]
        public FileResult Export()
        {
            
            DataTable dt = new DataTable("Drivers");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("DriverRefNo"),
                                            new DataColumn("DriverTitle")
                                        });

            var drivers = from customer in db.SP_DriverList()
                            select customer;

            foreach (var driver in drivers)
            {
                dt.Rows.Add(driver.DriverRefNo, driver.DriverTitle);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Driver.xlsx");
                }
            }
        }
    }
}
