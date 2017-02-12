using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Transportation.UnitTests
{
    public class KioskLogFixture : UnitFixture
    {
        //private KioskLog log;
        //[SetUp]
        //public void SetUp()
        //{
        //    log = new KioskLog();
        //}
        //[Test]
        //public void ApplyJson_SetsInterviewerID()
        //{
        //    var json = new JObject();
        //    json["interviewerId"] = 20;

        //    log.ApplyJson(json);
        //    Assert.AreEqual("20", log.InterviewerID);
        //}

        //[Test]
        //public void ApplyJson_SetsTerminalID()
        //{
        //    var json = new JObject();
        //    json["terminalId"] = 20;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(20, log.TerminalID);
        //}

        //[Test]
        //public void ApplyJson_SetsLocation()
        //{
        //    var json = new JObject();
        //    json["locationId"] = 10;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(10, log.LocationID);
        //}

        //[Test]
        //public void ApplyJson_SetsLatitude()
        //{
        //    var json = new JObject();
        //    json["latitude"] = "20.0231";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("20.0231", log.Latitude);
        //}

        //[Test]
        //public void ApplyJson_SeitsLongitude()
        //{
        //    var json = new JObject();
        //    json["longitude"] = "20.0231";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("20.0231", log.Longitude);
        //}

        //[Test]
        //public void ApplyJson_SetsFinishedTime()
        //{
        //    var json = new JObject();
        //    json["finishedTime"] = "2016/06/16 05:30:25";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("2016/06/16 05:30:25", log.FinishedTime);
        //}

        //[Test]
        //public void ApplyJson_SetsGEORequestedTime()
        //{
        //    var json = new JObject();
        //    json["geoRequestedTime"] = "2016/06/16 05:30:25";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("2016/06/16 05:30:25", log.GEORequestedTime);
        //}

        //[Test]
        //public void ApplyJson_SetsStartTime()
        //{
        //    var json = new JObject();
        //    json["startTime"] = "2016/06/16 05:30:25";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("2016/06/16 05:30:25", log.StartTime);
        //}

        //[Test]
        //public void ApplyJson_SetsEnterTime()
        //{
        //    var json = new JObject();
        //    json["enterTime"] = "2016/6/16 15:2:5";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("2016/6/16 15:2:5", log.EnterTime);
        //}

        //[Test]
        //public void ApplyJson_SetsLeaveTime()
        //{
        //    var json = new JObject();
        //    json["leaveTime"] = "2016/6/16 15:2:5";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("2016/6/16 15:2:5", log.LeaveTime);
        //}

        //[Test]
        //public void ApplyJson_SetsNumberOfCounter()
        //{
        //    var json = new JObject();
        //    json["numberOfCounter"] = 12;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(12, log.NumberOfCounter);
        //}

        //[Test]
        //public void ApplyJson_SetsQueueType()
        //{
        //    var json = new JObject();
        //    json["queueType"] = (int)QueueType.Bend;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(QueueType.Bend, log.QueueType);
        //}

        //[Test]
        //public void ApplyJson_SetsNumberOfPax()
        //{
        //    var json = new JObject();
        //    json["numberOfPax"] = 12;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(12, log.NumberOfPax);
        //}

        //[Test]
        //public void ApplyJson_SetsNumberOfReceipt()
        //{
        //    var json = new JObject();
        //    json["numberOfReceipt"] = 2;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(2, log.NumberOfReceipt);
        //}

        //[Test]
        //public void ApplyJson_SetsGender()
        //{
        //    var json = new JObject();
        //    json["gender"] = (int)Gender.Male;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(Gender.Male, log.Gender);
        //}
        //[Test]
        //public void ApplyJson_SetsColor()
        //{
        //    var json = new JObject();
        //    json["color"] = "FF00FF";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("FF00FF", log.Color);
        //}

        //[Test]
        //public void ApplyJson_SetsRemark1()
        //{
        //    var json = new JObject();
        //    json["remark1"] = "My remark 1";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("My remark 1", log.Remark1);
        //}

        //[Test]
        //public void ApplyJson_SetsRemark2()
        //{
        //    var json = new JObject();
        //    json["remark2"] = "My remark 2";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("My remark 2", log.Remark2);
        //}

        //[Test]
        //public void ApplyJson_SetsIsAbandon()
        //{
        //    var json = new JObject();
        //    json["isAbandon"] = true;

        //    log.ApplyJson(json);
        //    Assert.IsTrue(log.IsAbandon);
        //}

        //[Test]
        //public void ApplyJson_SetsAbandonReason()
        //{
        //    var json = new JObject();
        //    json["abandonReason"] = "Dislike";

        //    log.ApplyJson(json);
        //    Assert.AreEqual("Dislike", log.AbandonReason);
        //}

        //[Test]
        //public void ApplyJson_SetsShift()
        //{
        //    var json = new JObject();
        //    json["shift"] = 1;

        //    log.ApplyJson(json);
        //    Assert.AreEqual(1, log.Shift);
        //}
    }
}
