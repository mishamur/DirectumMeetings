using DirectumMeetings.Controllers;
using DirectumMeetings.Models;

namespace DirectumMeetingsTest
{
    public class MeetingsControllerTest
    {
        MeetingsController controller;
        [SetUp]
        public void Setup()
        {
            controller = new MeetingsController();

        }
        //���-�� controller.GetAllMeetings() �� ������ ����������
        [Test]
        public void DeleteNonExistingMeeting()
        {
            // Arrange
            var existingMeeting = new Meeting(DateTime.Now.AddDays(1), "Meeting 1", "Location 1", TimeSpan.FromHours(1));
            controller.AddMeeting(existingMeeting);
            var initialMeetingsCount = controller.GetAllMeetings().Count;
            // Act
            controller.RemoveMeeting("Non-existing Meeting");
            // Assert
            Assert.AreEqual(initialMeetingsCount, controller.GetAllMeetings().Count);
        }
        //�������� �� ���������� ���� ������� ������������ �� �������
        [Test]
        public void DontAddMeetingIfIntersect()
        {
            // Arrange
            var initialMeetingsCount = controller.GetAllMeetings().Count;
            var existingMeeting = new Meeting(DateTime.Now.AddDays(1), "Meeting 1", "Location 1", TimeSpan.FromHours(1));
            controller.AddMeeting(existingMeeting);

            // Act
            var intersectingMeeting = new Meeting(DateTime.Now.AddDays(1).AddHours(0.5), "Meeting 2", "Location 2", TimeSpan.FromHours(1));

            // Assert
            Assert.IsFalse(controller.AddMeeting(intersectingMeeting));
            Assert.AreEqual(initialMeetingsCount + 1, controller.GetAllMeetings().Count); // ��������, ��� ������� �� ���� ���������
        }
        //���� �������� ������� � ��� �� �������, �� ��
        [Test]
        public void DeleteIsCorrect()
        {
            // Arrange
            var meetingToDelete = new Meeting(DateTime.Now.AddDays(2), "Meeting to Delete", "Location", TimeSpan.FromHours(1));
            controller.AddMeeting(meetingToDelete);
            var initialMeetingsCount = controller.GetAllMeetings().Count;

            // Act
            controller.RemoveMeeting(meetingToDelete);

            // Assert
            Assert.AreEqual(initialMeetingsCount - 1, controller.GetAllMeetings().Count); // ��������, ��� ���������� ������ ����������� �� 1
            Assert.IsFalse(controller.IsContainsWithTitle("Meeting to Delete")); // ��������, ��� ��������� ������� ������ �� ���������� � ������
        }

        [Test]
        public void UpdateMeetingCorrectly()
        {
            // Arrange
            var meetingToUpdate = new Meeting(DateTime.Now.AddDays(3), "Meeting to Update", "Location", TimeSpan.FromHours(1));
            controller.AddMeeting(meetingToUpdate);
            var updatedMeeting = new Meeting(DateTime.Now.AddDays(3).AddHours(1), "Updated Meeting", "Updated Location", TimeSpan.FromHours(2));

            // Act
            controller.UpdateMeeting("Meeting to Update", updatedMeeting);

            // Assert
            var retrievedMeeting = controller.GetAllMeetings().ToList().Find(m => m.Title == "Updated Meeting");
            Assert.IsNotNull(retrievedMeeting); // ��������, ��� ������� ���� ������� ���������
            Assert.AreEqual(updatedMeeting.Title, retrievedMeeting.Title);
            Assert.AreEqual(updatedMeeting.Place, retrievedMeeting.Place);
            Assert.AreEqual(updatedMeeting.DateStart, retrievedMeeting.DateStart);
            Assert.AreEqual(updatedMeeting.DateEnd, retrievedMeeting.DateEnd);
            Assert.AreEqual(updatedMeeting.TimeDuration, retrievedMeeting.TimeDuration);
        }
        [Test]
        public void AddMeetingInPast()
        {
            // Arrange
            var initialMeetingsCount = controller.GetAllMeetings().Count;
            var pastMeeting = new Meeting(DateTime.Now.AddDays(-1), "Past Meeting", "Location", TimeSpan.FromHours(1));

            // Act
            bool isAdded = controller.AddMeeting(pastMeeting);

            // Assert
            Assert.IsFalse(isAdded);
            Assert.AreEqual(initialMeetingsCount, controller.GetAllMeetings().Count); // ���������, ��� ���������� ������ �� ����������
        }
    }
}