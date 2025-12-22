
using WorldOfZuul.Entities;
using WorldOfZuul.Items;

namespace WorldOfZuul.World
{
    /*
     * Builder for Department of Housing and Urban Development.
     * Contains following interactive objects: desk with three drawers, trash bin, filing cabinet, wall calendar.
     */
    public static class Office1RoomBuilder
    {
        public static Room BuildPoliticalOffice01()
        {
            var room = new Room(
                "Office 01",
                "Department of Housing and Urban Development",
                "The office of the Department of Housing and Urban Development is located on the third floor of the Palazzo Comunale." +
                "The walls are painted white with two beautiful old windows. " +
                "The windows look out onto a beautiful garden with a lake. " +
                "In the middle of the room is a metal desk with overflowing drawers. " +
                "Next to the windows is a wall calendar with appointments written on it. LED lamps hang from the ceiling. " +
                "You notice a metal trash can near the desk, a filing cabinet against the wall, and a calendar with repeated notes. " +
                "There are no signs of corruption, but something feels wrong. " +
                "\nINVESTIGATION\n" +
                "Find all evidence of corruption. Check the objects in the room:\n" +
                "the desk, the trash, the cabinet and the calendar.\n" +
                "You need all objects to complete your investigation and exit the room.\n" +
                "Useful commands: inspect [object], open [object], search [object], take [item]\n" +
                "Type 'help' for more guidance.\n"
            );

            // Add DESK with three drawers
            room.InteractiveObjects["desk"] = BuildDesk(room);

            // Add TRASH BIN (contains Rolex box)
            room.InteractiveObjects["trash"] = BuildTrashBin(room);

            // Add FILING CABINET (missing documents, "temporarily reassigned" files)
            room.InteractiveObjects["cabinet"] = BuildFilingCabinet();

            // Add WALL CALENDAR (recurring off-the-books meetings)
            room.InteractiveObjects["calendar"] = BuildWallCalendar();

            // Add exit back to Political Offices 
            room.Exits["Political Offices"] = new Exit("Political Offices", "Political Offices", room);
            return room;
        }

        private static InteractiveObject BuildDesk(Room room)
        {
            return new InteractiveObject(
                "desk",
                "Metal Desk",
                "A standard metal desk with three drawers.",
                "On the desk is a plant with a picture of two children. " +
                "The desk has three drawers, one on the left, one in the middle, and one on the right. " +
                "The middle drawer is locked.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A standard municipal desk with three drawers. You notice:\n" +
                                " • A Left drawer: a bit open, seems to contain files (try: open drawer left)\n" +
                                " • A Right drawer: closed, the handle shows fingerprint smudges (try: open drawer right)\n" +
                                " • A Center drawer: has newer lock (try: open drawer center)\n", null);

                    // LEFT DRAWER: Contains housing project files with repeated delays and handwritten "priority changes"
                    if (verb == "open drawer left" || verb == "open left drawer" || verb == "search drawer left")
                    {
                        if (state.GetFlag("desk_left_opened"))
                            return ("The left drawer is still open. You've already reviewed the housing project files inside.", null);

                        return (
                            "You opened the left drawer. " +
                            "It contains some documents about the housing projects. " +
                            "As you read through them, you notice that many construction projects are delayed. " +
                            "• Via Garibaldi Social Housing (Delayed 6 months). Handwritten note: 'Priority revised -> see meeting notes.'\n" + 
                            " • Poplar Reconstruction Quarters (Delayed 8 months). Handwritten note: 'Firm change -> Edilcoop preferred.'\n" + 
                            " • Affordable Housing Initiative: Zone 4 (Delayed 11 months). Stamped 'REASSIGNED' with no explanation.\n" +
                            "In most documents, the delay is due to a change in the construction company. " +
                            "In most cases, this change is in favor of the same construction company. " +
                            "The change from one construction company to another is usually not justified, which is very unusual. " +
                            "However, it may be that due to a shortage of staff or resources, there is only one company capable of carrying out the construction work for the municipality. " +
                            "Therefore, everything is legal so far.",
                            s => s.SetFlag("desk_left_opened")
                        );
                    }

                    // RIGHT DRAWER: Meeting notes with construction firms and vague promises
                    if (verb == "open drawer right" || verb == "open right drawer")
                    {
                        if (state.GetFlag("desk_right_opened"))
                        {
                            //Check if the notebook is still in the room
                            if (room.Items.ContainsKey("notebook"))
                                return ("The right drawer is still open. You can see the notebook with meeting notes inside.\n" +
                                "HINT: You can still take it with: take notebook", null);    
                            else
                            {
                                return ("The right drawer is opened and empty now." +
                                "The notebook is gone.", null);
                            }                   
                        }
                        return (
                            "You open the right drawer. " +
                            "In the right drawer are a few copies of restaurant receipts. Nothing unusual." +
                            "There's also a notebook with notes from conversations. " +
                            "The entries are dated, but the wording stays vague:\n" +
                            " • 14 Feb - Meeting with Edilcoop representatives. Project timelines discussed. = €8,500.\n" +
                            " • 3 Mar - Lunch with Bertolini Construction. Will accelerate permits. = €15,000\n" +
                            " • 22 Apr - Coffee with Assessore Mancini. Spoke him about delayed housing projects. Just agreed priority adjustments necessary.\n" +
                            " • 18 May - Dinner with Conti & Figli. Discussion of civic partnership. Generous contribution to local festival fund.\n" +
                            "None of this is illegal. Politicians meet with contractors all the time. But the wording and the money are odd: " +
                            "€8,500,' '€15,000,' 'generous contribution.' Why not just say what was discussed?\n" +
                            "You notice the meetings with Edilcoop and Bertolini align with the dates when certain housing projects were " +
                            "'reassigned' or 'priority revised' in the left drawer files. Coincidence? Maybe. Proof? No. \n" +
                            "EVIDENCE TIP: You can take the notebook as a evidence.",
                            s => 
                            {
                                s.SetFlag("desk_right_opened");
                                // Add Notebook to the room
                                if(!room.Items.ContainsKey("notebook"))
                                {
                                    room.SetItem(new Item(
                                        "notebook",
                                        "Meeting Notes Notebook",
                                        "A notebook containing vague notes from meetings with construction firms and politicians."
                                    ));
                                    // "take notebook" command will be available now
                                }
                            }
                        );
                    }

                    // CENTER DRAWER: Locked initially, but can be opened if player has searched other areas
                    if (verb == "open drawer center" || verb == "open center drawer" || verb == "search drawer center")
                    {
                        if (state.GetFlag("desk_center_opened"))
                            {
                            //Check if note is still in the room
                            if (room.Items.ContainsKey("note"))
                            {
                                return("The center drawer is already open. You can see the handwritten note inside.\n" +
                                "HINT: You can still take it with: take note", null);  
                            }
                            else
                            {
                                return ("The center drawer is opened and empty now." +
                                "The handwritten note is gone.", null);
                            }
                        }

                        if (!state.GetFlag("trash_searched") && !state.GetFlag("cabinet_searched"))
                            return ("The center drawer is locked. You can try the handle, but it doesn't budge. " +
                                    "You need to find something to open it, or perhaps understand the office better before forcing it.\n" +
                                    "HINT: Explore other objects in the room first.", null);

                        return (
                            "You try the center drawer again. This time it opens.\n" +
                            "Inside you find several personal documents.\n" +
                            "Inside you find:\n" +
                            " • Receipts for expensive dinners at Ristorante Savini in Milan, Hotel Excelsior in Rome, and a private golf club.\n" +
                            " • A partially visible bank statement. The monthly salary: €2,400. Additional deposits: €8,500 without any description and €6,200 labeled as 'consulting fee'.\n" +
                            " • A handwritten note on an official letterhead: 'Reminder: Festival fund donation €15,000(Conti & Figli). Permit approval by June.'\n" +
                            "None of these documents are illegal on their own. Municipal councillors may receive consulting fees, and companies can donate to public events. " +
                            "But the amounts do not match the declared income, and no invoices are attached.\n" +
                            "Compared to the other documents in the office, the picture becomes clearer. " +
                            "Project delays, the priority changes, informal meetings, additional income, and donations appear close in time.\n" +
                            "No single document proves wrongdoing. Taken together, they could suggest a system that operates quietly and consistently.\n" +
                            "EVIDENCE TIP: You can take the note as a evidence.",
                            s => 
                            {
                                s.SetFlag("desk_center_opened");
                                // Add Handwritten note to the room
                                if(!room.Items.ContainsKey("note"))
                                {
                                    room.SetItem(new Item(
                                        "note",
                                        "Handwritten Note on Letterhead",
                                        "A handwritten note on official letterhead mentioning a festival fund donation and permit approval."
                                    ));
                                }
                            }
                        );
                    }

                    return ("You can't do that with the desk.",  null);
                }
            );
        }

        private static InteractiveObject BuildTrashBin(Room room)
        {
            return new InteractiveObject(
                "trash",
                "Metal Trash Bin",
                "A small metal trash bin beside the desk.",
                "A standard office trash bin. Someone empties it regularly, because it is mostly empty.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "search trash" || verb == "look in trash")
                    {
                        if (state.GetFlag("trash_searched"))
                        {
                            //Check if the rolex box is still in the room
                            if (room.Items.ContainsKey("rolex_box"))
                                return ("You've already searched the trash bin. The empty Rolex box is still inside.\n" +
                                "HINT: You can still take it with: take rolex box", null);    
                            else
                            {
                                return ("The trash bin is empty now." +
                                "The Rolex box is gone.", null);
                            }
                        }
                        return (
                            "You look inside the trash bin. Most of it is ordinary office waste: some sheets and an empty espresso cup.\n" +
                            "At the bottom of the bin you notice a small box.\n" +
                            "It is an empty Rolex box. " +
                            "The box includes a certificate paper and a small card that reads: 'Con gratitudine: Edilcoop.'\n" +
                            "Edilcoop is the same construction company that appears repeatedly in the housing project documents. " +
                            "It is also the company whose projects were often given revised priorities.\n" +
                            "On its own, this does not prove anything. Gifts are not illegal if declared, and there is no watch here, only the packaging. " +
                            "The item raises questions, but provides no clear answer. " +
                            "EVIDENCE TIP: You can take the Rolex Box as a evidence.",
                            s => 
                            {
                                s.SetFlag("trash_searched");
                                // Add Rolex Box to the room
                                if(!room.Items.ContainsKey("rolex_box"))
                                {
                                    room.SetItem(new Item(
                                        "rolex_box",
                                        "Rolex Box",
                                        "An empty Rolex box with a certificate. It`s from Edilcoop."
                                    ));
                                    // "take rolex box" command will be available now
                                    room.Items["rolex box"] = room.Items["rolex_box"];  
                                }
                            }

                        );
                    }

                    return ("The trash bin sits silently, keeping its secrets.", null);
                }
            );
        }

        private static InteractiveObject BuildFilingCabinet()
        {
            return new InteractiveObject(
                "cabinet",
                "Filing Cabinet",
                "A tall metal filing cabinet containing municipal housing files.",
                "A gray metal filing cabinet with 4 drawers They are organized by year and project category. " +
                "Some of the drawers are filled with thick folders, while others contains only a few documents.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A filing cabinet containing documents on municipal housing projects. The drawers are clearly labeled by year and topic, " +
                                "including 'Social Housing 1989', 'Permits and Approvals 1990/91', and 'Public Contracts 1992'.\n" +
                                "You can open or search the cabinet to look through the files in more detail.", null);

                    if (verb == "open cabinet" || verb == "search cabinet")
                    {
                        if (state.GetFlag("cabinet_searched"))
                            return ("You've already searched the cabinet. The missing files and 'temporarily reassigned' notices are still gone.", null);

                        return (
                            "You open the filing cabinet and go through the folders one by one. " +
                            "Most of them contains standard administrative documents such as zoneing permits, construction schedules, " +
                            "contractor agreements, and environmental reports.\n" +
                            "After a while, you began to notice inconsistencies.\n" +
                            " • A labeled folder 'Via Mazzini Public Housing Tender Process' is empty. Inside, a short card states: 'Temporarily reassigned to regional office.'\n" +
                            " • A labeled folder 'Edilcoop Contract Review from 1991' appears incomplete. The index lists 47 pages, but there are only 12 pages present.\n" +
                            " • A labeled folder 'Complaint Log: Citizen Petitions' contains only a few pages. A note attached reads: 'Archive after resolution!'\n" +
                            "The missing documents are related to the key decisions, such as contractor changes, project reassignments, and citizen complaints. " +
                            "However, no explanations are included.\n" +
                            "The remaining files are formally correct, stamped and signed, but provide little information about how decisions were made. " +
                            "Each gap is accompanied by a standard note such as 'regional office', 'under review', or 'archived'.\n" +
                            "The documents have been not gone. They have been moved, reassigned, or closed without any detail.",
                            s => s.SetFlag("cabinet_searched")
                        );
                    }

                    return ("The filing cabinet remains closed and guarding its bureaucratic secrets.", null);
                }
            );
        }

        private static InteractiveObject BuildWallCalendar()
        {
            return new InteractiveObject(
                "calendar",
                "Wall Calendar",
                "A wall calendar for 1992 with lots of handwritten entries.",
                "A wall calendar hanging next to the window. The pages contain appointments, meetings, and short notes written in small, hurried handwriting. " +
                "Several entries are marked with red circles.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "read calendar")
                    {
                        if (state.GetFlag("calendar_read"))
                            return ("You've already examined the calendar. The recurring late-night meetings at Ristorante Il Gabbiano are still circled in red.", null);

                        return (
                            "You look more closely at the calendar and read through the handwritten notes. " +
                            "Most of them are routine appointments such as 'Budget review at 10:00', 'Council meeting at 15:30', or 'Permit desk at 09:00'.\n" +
                            "Some entries, marked with red circles, appear at regular intervals.\n" +
                            " • 'Cena Il Gabbiano at 21:30'\n" +
                            " • Dates include January 14, January 28, February 11, February 25, March 10, March 24, April 7, and April 21.\n" +
                            "The location and time remain the same. The entries are always outside office hours and are not labeled as official meetings.\n" +
                            "When compared with the meeting notes and project files, several of these dates correspond closely with later changes in housing projects. " +
                            "Priority revisions and contractor reassignments occur shortly after some of the listed dinners.\n" +
                            "There is nothing illegal about these entries. Meetings over dinner are common, and decisions often follow informal discussions. " +
                            "However, these appointments do not appear in any official records.\n" +
                            "The calendar does not provide answers. It only shows timing, repetition, and proximity to later decisions."
,
                            s => s.SetFlag("calendar_read")
                        );
                    }

                    return ("The calendar hangs on the wall: Silent and annotated.", null);
                }
            );
        }



    }
}
