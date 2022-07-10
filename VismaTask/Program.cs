using VismaTask;

var meetings = new Meetings();

//user input variablies
string[] result;
string userInput;

//delimeters
string[] delimeters = new string[] { " | ", "| ", " |", "|" };

//loads a save file
meetings.load();

Console.WriteLine("Type /help for help.");

while (true)
{
    userInput = "";

    while (userInput == "")
    {
        userInput = Console.ReadLine();
    }

    result = userInput.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

    switch (result[0])
    {
        case "/addMeeting":

            if (result.Count() == 8)
            {

                var name = result[1];
                var responsiblePerson = result[2];
                var description = result[3];
                int category = 0;
                var type = 0;
                var startDate = result[6];
                var endDate = result[7];


                if (result[4] == "CodeMonkey")
                {
                    category = 0;
                }
                else if (result[4] == "Hub")
                {
                    category = 1;
                }
                else if (result[4] == "Short")
                {
                    category = 2;
                }
                else if (result[4] == "TeamBuilding")
                {
                    category = 3;
                }
                else
                {
                    meetings.TerminalResponse("Category error. Categories: CodeMonkey, Hub, Short, TeamBuilding", "red");
                    break;
                }


                if (result[5] == "Live")
                {
                    type = 0;
                }
                else if (result[5] == "InPerson")
                {
                    type = 1;
                }
                else
                {
                    meetings.TerminalResponse("Type error. Types: Live, InPerson", "red");
                    break;
                }


                if (!meetings.validDate(result[6]) || !meetings.validDate(result[7]))
                {
                    break;
                }

                var newMeeting = new Meeting(name, responsiblePerson, description, category, type, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                meetings.AddMeeting(newMeeting);

                break;
            }

            meetings.TerminalResponse("Syntax error. Use: /addMeeting|name|responsiblePerson|description|category|type|startDate|endDate|", "red");
            break;

        case "/list":

            if (result.Count() < 6)
            {
                var filter = "";
                var target = "";
                var startDate = Convert.ToDateTime("2020-10-10");
                var endDate = Convert.ToDateTime("2020-10-10"); ;

                //lists all meetings
                if (result.Count() == 1)
                {
                    meetings.DisplayMeetings(null, null, startDate, endDate);
                    break;
                }
                //lists meetings when filtering descrpition, responsiblePerson, category, type, attendees.
                else if (result.Count() == 3)
                {
                    if (result[1] == "description" || result[1] == "responsiblePerson" || result[1] == "category" || result[1] == "type" || result[1] == "attendees")
                    {
                        filter = result[1];
                        target = result[2];
                    }
                    else
                    {
                        meetings.TerminalResponse("Syntax error. Use: /list", "red");
                        break;
                    }

                    meetings.DisplayMeetings(filter, target, startDate, endDate);
                    break;
                }
                //lists all meetings that start from specific date
                else if (result.Count() == 4)
                {
                    if (result[1] == "date" && result[2] == "from")
                    {
                        filter = result[1];
                        target = result[2];

                        if (!meetings.validDate(result[3]))
                        {
                            break;
                        }

                        startDate = Convert.ToDateTime(result[3]);

                        meetings.DisplayMeetings(filter, target, startDate, endDate);
                        break;

                    }
                    else
                    {
                        meetings.TerminalResponse("Syntax error. Use: /list", "red");
                        break;
                    }

                }
                //lists all meetings that start from specific date
                else if (result.Count() == 5)
                {
                    if (result[1] == "date" && result[2] == "between")
                    {
                        filter = result[1];
                        target = result[2];

                        if (!meetings.validDate(result[3]) || !meetings.validDate(result[4]))
                        {
                            break;
                        }

                        startDate = Convert.ToDateTime(result[3]);
                        endDate = Convert.ToDateTime(result[4]);

                        meetings.DisplayMeetings(filter, target, startDate, endDate);
                        break;

                    }
                    else
                    {
                        meetings.TerminalResponse("Syntax error. Use: /list", "red");
                        break;
                    }
                }
                else
                {
                    meetings.TerminalResponse("Syntax error. Use: /list", "red");
                    break;
                }
            }

            meetings.TerminalResponse("Syntax error. Use: /list", "red");
            break;

        case "/deleteMeeting":

            if (result.Count() == 3)
            {
                var personName = result[1];
                var meetName = result[2];

                meetings.DeleteMeeting(personName, meetName);
                break;
            }

            meetings.TerminalResponse("Syntax error. Use: /deleteMeeting|yourName|meetingName|", "red");
            break;
        //no
        case "/addPerson":

            if (result.Count() == 4)
            {
                var personName = result[1];
                var meetName = result[2];
                var startTime = result[3];

                if (!meetings.validDate(result[3]))
                {
                    break;
                }

                meetings.AddPeople(meetName, personName, false, Convert.ToDateTime(startTime));
                break;
            }

            meetings.TerminalResponse("Syntax error. Use: /addperson|personName|meetingName|startTime|", "red");
            break;

        case "/deletePerson":

            if (result.Count() == 3)
            {
                var personName = result[1];
                var meetName = result[2];

                meetings.RemovePeople(personName, meetName);
                break;
            }

            meetings.TerminalResponse("Syntax error. Use: /deletePerson|personName|meetingName|", "red");
            break;


        case "/save":

            meetings.save();
            break;

        case "/load":

            meetings.load();
            break;

        case "/exit":

            meetings.save();
            return;

        case "/help":

            Console.WriteLine("/addMeeting|meetingName|responsiblePerson|description|category|type|startTime|endtime|");
            Console.WriteLine("/addPerson|personName|meetingName|startTime|");
            Console.WriteLine("/deleteMeeting|personName|meetingName|");
            Console.WriteLine("/deletePerson|personName|meetingName|");
            Console.WriteLine("/list");
            Console.WriteLine("/list|");
            Console.WriteLine("/save");
            Console.WriteLine("/load");
            Console.WriteLine("/exit");
            break;

        default:
            Console.WriteLine("Error. Type /help for help");
            break;
    }
}
