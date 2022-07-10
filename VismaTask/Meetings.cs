using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VismaTask
{
    public class Meetings
    {
        private List<Meeting> _meetings { get; set; } = new List<Meeting>();

        //save file name
        string fileName = "Meetings.json";

        //used for creating save files
        public void save()
        {
            string jsonString = JsonConvert.SerializeObject(_meetings);

            File.WriteAllText(fileName, jsonString);

            TerminalResponse("Saved.", "green");
        }

        //used for opening save files
        public void load()
        {
            //checks if save fie exists
            if (File.Exists(fileName))
            {
                StreamReader reader = new StreamReader(fileName);

                string jsonData = reader.ReadToEnd();

                var meetings = JsonConvert.DeserializeObject<List<Meeting>>(jsonData);


                _meetings.Clear();


                foreach (var meeting in meetings)
                {
                    _meetings.Add(meeting);
                }

                reader.Close();

                TerminalResponse("Save file loaded.", "green");
            }
        }

        //used for cmd responses
        public void TerminalResponse(string message, string type)
        {
            if (type == "red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }

        //used for adding meetings
        public void AddMeeting(Meeting meeting)
        {
            //checks if there is a meeting with the same name
            if (!meetingExists(meeting.Name))
            {
                //checks if the dates make sense
                if (meeting.StartDate < meeting.EndDate)
                {
                    //creates a meeting
                    _meetings.Add(meeting);

                    //adds a responsible person to the meetings atendees list
                    AddPeople(meeting.Name, meeting.ResponsiblePerson, true, meeting.StartDate);

                    TerminalResponse("Meeting was created successfully.", "green");
                }
                else
                {
                    TerminalResponse("Invalid Dates.", "red");
                }
            }
            else
            {
                TerminalResponse("Meeting with this name already exists.", "red");
            }
        }

        //used for displaying a certain meeting
        public void DisplayMeet(Meeting meeting)
        {
            Console.WriteLine($"Meeting: {meeting.Name}, {meeting.ResponsiblePerson}, {meeting.Description}, {meeting.Category}, {meeting.Type}, {meeting.StartDate}, {meeting.EndDate}");
        }

        //logic for displaying meetings
        public void DisplayMeetings(string filter, string target, DateTime startDate, DateTime endDate)
        {
            switch (filter)
            {
                case null:

                    foreach (var meeting in _meetings)
                    {
                        DisplayMeet(meeting);
                    }
                    break;

                case "description":

                    foreach (var meeting in _meetings)
                    {
                        if (meeting.Description.Contains(target))
                        {
                            DisplayMeet(meeting);
                        }
                    }
                    break;

                case "responsiblePerson":

                    foreach (var meeting in _meetings)
                    {
                        if (meeting.ResponsiblePerson.Contains(target))
                        {
                            DisplayMeet(meeting);
                        }
                    }
                    break;

                case "type":

                    
                    foreach (var meeting in _meetings)
                    {
                        if (meeting.Type.ToString().Contains(target))
                        {
                            DisplayMeet(meeting);
                        }
                    }
                    break;

                case "category":

                    foreach (var meeting in _meetings)
                    {
                        if (meeting.Category.ToString().Contains(target))
                        {
                            DisplayMeet(meeting);
                        }
                    }
                    break;

                case "date":

                    switch (target)
                    {
                        case "from":

                            foreach (var meeting in _meetings)
                            {
                                if (meeting.StartDate >= startDate)
                                {
                                    DisplayMeet(meeting);
                                }
                            }
                            break;

                        case "between":

                            foreach (var meeting in _meetings)
                            {
                                if (meeting.StartDate >= startDate && meeting.EndDate <= endDate)
                                {
                                    DisplayMeet(meeting);
                                }
                            }
                            break;
                    }
                    break;

                case "attendees":

                    foreach (var meeting in _meetings)
                    {
                        if (meeting.People.Count > Convert.ToInt32(target))
                        {
                            DisplayMeet(meeting);
                        }
                    }
                    break;
            }
        }

        //adding people to the meeeting
        public void AddPeople(string meetName, string personName, bool personResponsible, DateTime personStartTime)
        {
            //checks if the given meeting name exists
            if (meetingExists(meetName))
            {
                //finds the index of the desired meeting
                int meetId = meetingIndex(meetName);

                //gets the meeting start time and end time
                DateTime meetStartTime = _meetings[meetId].StartDate;
                DateTime meetEndTime = _meetings[meetId].EndDate;

                //person ends meeting when the time runs out
                DateTime personEndTime = meetEndTime;

                //checks if person that we are trying to add, already participates in this meeting
                if (!personExistsInMeeting(meetId, personName))
                {
                    //checks if meeting did start or it has already ended
                    if (meetStartTime <= personStartTime && meetEndTime > personStartTime)
                    {
                        //loops thru all of the meetings
                        foreach (var meeting in _meetings)
                        {
                            //checks if person has another meetings 
                            if (meeting.People.Exists(i => i.Name == personName))
                            {
                                //gets person id
                                int personId = meeting.People.FindIndex(i => i.Name == personName);

                                DateTime comparisonStartTime = meeting.People[personId].StartTime;
                                DateTime comparisonEndTime = meeting.People[personId].EndTime;

                                //checks if they intersect:

                                //     meeting starts while another meeting is happening             meeting ends while another meeting is happening                   meeting is over other meeting
                                if ((comparisonStartTime <= personStartTime && personStartTime < comparisonEndTime) || (comparisonStartTime < personEndTime && personEndTime <= comparisonEndTime) || (personStartTime < comparisonStartTime && personEndTime > comparisonEndTime))
                                {
                                    TerminalResponse($"WARNING: Person is already in {meeting.Name} meeting.", "red");
                                }
                            }
                        }

                        //creates new person var
                        var newPerson = new Person(personName, personResponsible, personStartTime, personEndTime);

                        _meetings[meetId].People.Add(newPerson);

                        //only prints it out, when person was added not by adding a meeting
                        if (!personResponsible)
                        {
                            TerminalResponse("Person added succesfully.", "green");
                        }
                    }
                    else
                    {
                        TerminalResponse("Dates do not match.", "red");
                    }
                }
                else
                {
                    TerminalResponse("Person already exists in this meeting", "red");
                }
            }
            else
            {
                TerminalResponse("Meeting with this name doesnt exist.", "red");
            }
        }

        //removing people from the meeting
        public void RemovePeople(string personName, string meetName)
        {
            //checks if the given meeting name exists
            if (meetingExists(meetName))
            {
                //finds the index of meeting
                int meetId = meetingIndex(meetName);

                //checks if person that we are trying to remove, participates in this meeting and he is not responsible for it
                if (personExistsInMeeting(meetId, personName) && !personIsResponsible(meetName, personName))
                {
                    //gets the person id and removes it
                    int personId = _meetings[meetId].People.FindIndex(i => i.Name == personName);

                    _meetings[meetId].People.RemoveAt(personId);

                    TerminalResponse("Person removed succesfully.", "green");
                }
                else
                {
                    TerminalResponse("Person doesnt exist or, he is responsible for this meeting.", "red");
                }
            }
            else
            {
                TerminalResponse("Meeting with this name doesnt exist.", "red");
            }
        }

        public void DeleteMeeting(string personName, string meetName)
        {
            //checks if the meeting name exists and person is responsible for it
            if (meetingExists(meetName) && personIsResponsible(meetName, personName))
            {
                //finds the id of the meeting
                int meetId = meetingIndex(meetName);

                _meetings.RemoveAt(meetId);

                TerminalResponse("Meeting deleted successfully.", "green");
            }
            else
            {
                TerminalResponse("Meeting doesnt exist, or you aren't the responsible person.", "red");
            }
        }

        //checks if person is responsible for the meeting
        public bool personIsResponsible(string meetName, string personName)
        {
            if (_meetings.Exists(i => i.ResponsiblePerson == personName && i.Name == meetName))
            {
                return true;
            }
            return false;
        }

        //checks if person exists in the meeting
        public bool personExistsInMeeting(int meetId, string personName)
        {

            if (_meetings[meetId].People.Exists(i => i.Name == personName))
            {
                return true;
            }
            return false;
        }

        //checks if the meeting exists
        public bool meetingExists(string meetName)
        {
            if (_meetings.Exists(i => i.Name == meetName))
            {
                return true;
            }
            return false;
        }

        //gets the desired meeting Id
        public int meetingIndex(string meetName)
        {
            return _meetings.FindIndex(i => i.Name == meetName);
        }

        //checks if the given date format is correct
        public bool validDate(string date)
        {
            DateTime temp;

            if (DateTime.TryParse(date, out temp))
            {
                return true;
            }

            TerminalResponse("Date format error. Date format: YYYY-MM-dd HH:mm", "red");
            return false;
        }
    }
}
