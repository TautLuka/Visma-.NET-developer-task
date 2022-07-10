using VismaTask;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void ValidDate_Valid_ReturnsTrue()
        {
            //arrange
            var meetings = new Meetings();

            //act
            var result = meetings.ValidDate("2020-10-10");

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ValidDate_Invalid_ReturnsFalse()
        {
            //arrange
            var meetings = new Meetings();

            //act
            var result = meetings.ValidDate("2020-10-10 01234");

            //assert
            Assert.False(result);
        }

        [Fact]
        public void MeetingExists_Exists_ReturnsTrue()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);
            
            //assert
            Assert.True(meetings.MeetingExists(meeting.Name));
        }

        [Fact]
        public void MeetingExists_DoesntExist_ReturnsFalse()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);

            //assert
            Assert.False(meetings.MeetingExists("problem"));
        }

        [Fact]
        public void MeetingIndex_1_1()
        {
            //arrange
            var meetings = new Meetings();
            var meeting0 = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));
            var meeting1 = new Meeting("Solve", "Julius", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting0);
            meetings.AddMeeting(meeting1);

            //assert
            Assert.Equal(1, meetings.MeetingIndex(meeting1.Name));
        }

        [Fact]
        public void PersonExistsInMeeting_Exists_ReturnsTrue()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));
            
            //act
            meetings.AddMeeting(meeting);
            
            //assert
            Assert.True(meetings.PersonExistsInMeeting(0, meeting.ResponsiblePerson));
        }

        [Fact]
        public void PersonExistsInMeeting_DoesntExist_ReturnsFalse()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);

            //assert
            Assert.False(meetings.PersonExistsInMeeting(0, "Julius"));
        }

        [Fact]
        public void PersonIsResponsible_Responsible_ReturnsTrue()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);

            //assert
            Assert.True(meetings.PersonIsResponsible(meeting.Name, meeting.ResponsiblePerson));
        }

        [Fact]
        public void PersonIsResponsible_NotResponsible_ReturnsFalse()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);

            //assert
            Assert.False(meetings.PersonIsResponsible(meeting.Name, "Julius"));
        }

        [Fact]
        public void DeleteMeeting_EverythingIsCorrect_DeletesTheMeeting()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);
            meetings.DeleteMeeting(meeting.ResponsiblePerson, meeting.Name);

            //assert
            Assert.False(meetings.MeetingExists(meeting.Name));
        }

        [Fact]
        public void DeleteMeeting_MeetingNameIncorrect_DoesntDeleteTheMeeting()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);
            meetings.DeleteMeeting(meeting.ResponsiblePerson, "Solve");

            //assert
            Assert.True(meetings.MeetingExists(meeting.Name));
        }

        [Fact]
        public void DeleteMeeting_PersonNameIncorrect_DoesntDeleteTheMeeting()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));

            //act
            meetings.AddMeeting(meeting);
            meetings.DeleteMeeting("Julius", meeting.Name);

            //assert
            Assert.True(meetings.MeetingExists(meeting.Name));
        }

        [Fact]
        public void RemovePerson_EverythingIsCorrect_PersonRemoved()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));
            

            //act
            meetings.AddMeeting(meeting);
            meetings.AddPeople(meeting.Name, "Julius", false, Convert.ToDateTime("2020-10-11"));
            meetings.RemovePeople("Julius", meeting.Name);

            //assert
            Assert.False(meetings.PersonExistsInMeeting(0, "Julius"));
        }

        [Fact]
        public void RemovePerson_PersonNameIncorrect_DoesntRemove()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));


            //act
            meetings.AddMeeting(meeting);
            meetings.AddPeople(meeting.Name, "Julius", false, Convert.ToDateTime("2020-10-11"));
            meetings.RemovePeople("Dovydas", meeting.Name);

            //assert
            Assert.True(meetings.PersonExistsInMeeting(0, "Julius"));
        }

        [Fact]
        public void RemovePerson_MeetingNameIncorrect_DoesntRemove()
        {
            //arrange
            var meetings = new Meetings();
            var meeting = new Meeting("Problem", "Tautvydas", "description", 0, 0, Convert.ToDateTime("2020-10-10"), Convert.ToDateTime("2020-10-15"));


            //act
            meetings.AddMeeting(meeting);
            meetings.AddPeople(meeting.Name, "Julius", false, Convert.ToDateTime("2020-10-11"));
            meetings.RemovePeople("Julius", "Solve");

            //assert
            Assert.True(meetings.PersonExistsInMeeting(0, "Julius"));
        }

    }
}
