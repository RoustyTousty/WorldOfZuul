using WorldOfZuul.Items;
using WorldOfZuul.Entities;

namespace WorldOfZuul.World
{
    /*
     * Builder for Political Office 01 — Housing Councillor's Office, Italy, early 1990s (Mani Pulite era).
     * Contains interactive objects: desk with three drawers, trash bin, filing cabinet, wall calendar.
     */
    public static class Office1RoomBuilder
    {
        public static Room BuildPoliticalOffice01()
        {
            var room = new Room(
                "office_01",
                "Office 01 – Housing Councillor",
                "The office of the Department of Housing and Urban Development is located on the third floor of the Palazzo Comunale." +
                "The walls are painted white with two beautiful old windows." +
                "The windows look out onto a beautiful garden with a lake. " +
                "In the middle of the room is a metal desk with overflowing drawers." +
                "Next to the windows is a wall calendar with appointments written on it. LED lamps hang from the ceiling." +
                "You notice a metal trash can near the desk, a filing cabinet against the wall, and a calendar with repeated notes. " +
                "There are no signs of corruption, but something feels wrong." +
                "INTERACTIVE OBJECTS IN THIS ROOM:\n" +
                " • desk - A standard metal desk with three drawers (try: inspect desk, open drawer left, open drawer right, open drawer center)\n" +
                " • trash - A metal trash bin beside the desk (try: inspect trash, search trash)\n" +
                " • cabinet - A tall filing cabinet with municipal housing files (try: inspect cabinet, open cabinet, search cabinet)\n" +
                " • calendar - A wall calendar with repeated annotations (try: inspect calendar, read calendar)\n"
            );

            // Add DESK with three drawers
            room.InteractiveObjects["desk"] = BuildDesk();

            // Add TRASH BIN (contains Rolex box)
            room.InteractiveObjects["trash"] = BuildTrashBin();

            // Add FILING CABINET (missing documents, "temporarily reassigned" files)
            room.InteractiveObjects["cabinet"] = BuildFilingCabinet();

            // Add WALL CALENDAR (recurring off-the-books meetings)
            room.InteractiveObjects["calendar"] = BuildWallCalendar();

            return room;
        }

        private static InteractiveObject BuildDesk()
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
                                " • Left drawer: slightly open, seems to contain files\n" +
                                " • Right drawer: closed, handle shows fingerprint smudges\n" +
                                " • Center drawer: newer lock\n", null);

                    // LEFT DRAWER: Housing project files with repeated delays and handwritten "priority changes"
                    if (verb == "open drawer left" || verb == "open left drawer")
                    {
                        if (state.GetFlag("desk_left_opened"))
                            return ("The left drawer is still open. You've already reviewed the housing project files inside.", null);

                        return (
                            "You opened the left drawer. " +
                            "It contains some documents about the housing projects. " +
                            "As you read through them, you notice that many construction projects are delayed. " +
                            "• Via Garibaldi Social Housing (Delayed 6 months). Handwritten note: 'Priority revised -> see meeting notes.'\n" + 
                            " • Quartiere Popolare Reconstruction (Delayed 8 months). Handwritten note: 'Firm change — Edilcoop preferred.'\n" + 
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
                            return ("The right drawer is still open. The meeting notes are still there, vague and non-committal.", null);

                        return (
                            "You open the right drawer. " +
                            "In the right drawer are a few copies of restaurant receipts. Nothing unusual." +
                            "There's also a notebook with notes from conversations. " +
                            "The entries are dated, but the wording stays vague:\n" +
                            " • 14 Feb - Meeting with Edilcoop representatives. Project timelines discussed. = €8,500.\n" +
                            " • 3 Mar - Lunch with Bertolini Construction. Will expedite permits. = €15,000\n" +
                            " • 22 Apr - Coffee with Assessore Mancini. Spoke of delayed housing projects. Agreed priority adjustments necessary.\n" +
                            " • 18 May - Dinner with Conti & Figli. Discussed civic partnership. Generous contribution to local festival fund.\n" +
                            "None of this is illegal. Politicians meet with contractors all the time. But the wording and the money are odd: " +
                            "€8,500,' '€15,000,' 'generous contribution.' Why not just say what was discussed?\n" +
                            "You notice the meetings with Edilcoop and Bertolini align with the dates when certain housing projects were " +
                            "'reassigned' or 'priority revised' in the left drawer files. Coincidence? Maybe. Proof? No.",
                            s => s.SetFlag("desk_right_opened")
                        );
                    }

                    // CENTER DRAWER: Locked initially, can be opened if player has searched other areas
                    if (verb == "open drawer center" || verb == "open center drawer")
                    {
                        if (state.GetFlag("desk_center_opened"))
                            return ("The center drawer is already open. Inside are personal receipts and a bank statement with unexplained deposits.", null);

                        if (!state.GetFlag("trash_searched") && !state.GetFlag("cabinet_searched"))
                            return ("The center drawer is locked. You try the handle, but it doesn't budge. " +
                                    "You'd need to find something to open it, or perhaps understand the office better before forcing it.\n" +
                                    "HINT: Explore other objects in the room first.", null);

                        return (
                            "You try the center drawer again. This time it opens.\n" +
                            "Inside you find several personal documents.\n" +
                            "Inside you find:\n" +
                            " • Receipts for expensive dinners at Ristorante Savini in Milan, Hotel Excelsior in Rome, and a private golf club.\n" +
                            " • A partially visible bank statement. Monthly salary: €2,400. Additional deposits: €8,500 without description and €6,200 labeled as 'consulting fee'.\n" +
                            " • A handwritten note on official letterhead: 'Reminder: Festival fund donation €15,000(Conti & Figli). Permit approval by June.'\n" +
                            "None of these documents are illegal on their own. Municipal councillors may receive consulting fees, and companies can donate to public events. " +
                            "However, the amounts do not clearly match the declared income, and no invoices are attached.\n" +
                            "When compared with the other documents in the office, the picture becomes clearer. " +
                            "Project delays, priority changes, informal meetings, additional income, and donations appear close in time.\n" +
                            "No single document proves wrongdoing. Taken together, they suggest a system that operates quietly and consistently.",
                            s => s.SetFlag("desk_center_opened")
                        );
                    }

                    return ("You can't do that with the desk.", null);
                }
            );
        }

        private static InteractiveObject BuildTrashBin()
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
                            return ("You've already searched the trash. The empty Rolex box is still there", null);

                        return (
                            "You look inside the trash bin. Most of it is ordinary office waste: crumpled notes and an empty espresso cup.\n" +
                            "At the bottom of the bin, partly covered by paper, you notice a small box.\n" +
                            "It is an empty Rolex box. The packaging is intact, but there is no watch inside. " +
                            "The box includes a certificate of authenticity with no name filled in and a small card that reads: 'Con gratitudine — Edilcoop.'\n" +
                            "Edilcoop is the same construction company that appears repeatedly in the housing project documents. " +
                            "It is also the company whose projects were often given revised priorities.\n" +
                            "On its own, this does not prove anything. Gifts are not illegal if declared, and there is no watch here, only the packaging. " +
                            "The box may have been discarded after a refusal, stored separately, or intended for someone else.\n" +
                            "There is no way to know. The item raises questions, but provides no clear answer.",
                            s => s.SetFlag("trash_searched")
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
                "A gray metal filing cabinet with four drawers, organized by year and project category. " +
                "Some drawers are filled with thick folders, while others contain only a few documents.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A filing cabinet containing municipal housing project files. The drawers are clearly labeled by year and topic, " +
                                "including 'Social Housing 1989', 'Permits and Approvals 1990–91', and 'Public Contracts 1992'.\n" +
                                "You can open or search the cabinet to look through the files in more detail.", null);

                    if (verb == "open cabinet" || verb == "search cabinet")
                    {
                        if (state.GetFlag("cabinet_searched"))
                            return ("You've already searched the cabinet. The missing files and 'temporarily reassigned' notices are still gone.", null);

                        return (
                            "You open the filing cabinet and go through the folders one by one. " +
                            "Most of them contain standard administrative documents such as zoning permits, construction schedules, " +
                            "contractor agreements, and environmental reports.\n" +
                            "After a while, you begin to notice inconsistencies.\n" +
                            " • A folder labeled 'Via Mazzini Public Housing Tender Process' is empty. Inside, a short note states: 'Temporarily reassigned to regional office.'\n" +
                            " • A folder labeled 'Edilcoop Contract Review from 1991' appears incomplete. The index lists 47 pages, but only 12 are present.\n" +
                            " • A folder labeled 'Complaint Log: Citizen Petitions' contains only a few pages. A note attached reads: 'Archive after resolution.'\n" +
                            "The missing documents are related to key decisions, such as contractor changes, project reassignments, and citizen complaints. " +
                            "However, no explanations are included.\n" +
                            "The remaining files are formally correct, stamped and signed, but provide little information about how decisions were made. " +
                            "Each gap is accompanied by a standard note such as 'regional office', 'under review', or 'archived'.\n" +
                            "The documents have not disappeared. They have been moved, reassigned, or closed without further detail.",
                            s => s.SetFlag("cabinet_searched")
                        );
                    }

                    return ("The filing cabinet remains closed, guarding its bureaucratic secrets.", null);
                }
            );
        }

        private static InteractiveObject BuildWallCalendar()
        {
            return new InteractiveObject(
                "calendar",
                "Wall Calendar",
                "A wall calendar for 1992 with numerous handwritten entries.",
                "A wall calendar hanging next to the window. The pages contain appointments, meetings, and short notes written in small, hurried handwriting. " +
                "Several entries are marked with red circles.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "read calendar")
                    {
                        if (state.GetFlag("calendar_read"))
                            return ("You've already examined the calendar. The recurring late-night meetings at Ristorante Il Gabbiano are still circled in red.", null);

                        return (
                            "You look more closely at the calendar and read through the handwritten entries. " +
                            "Most of them are routine appointments such as 'Budget review at 10:00', 'Council meeting at 15:30', and 'Permit desk at 09:00'.\n" +
                            "Some entries, marked with red circles, appear at regular intervals.\n" +
                            " • 'Cena Il Gabbiano at 21:30'\n" +
                            " • Dates include Jan 14, Jan 28, Feb 11, Feb 25, Mar 10, Mar 24, Apr 7, and Apr 21.\n" +
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

                    return ("The calendar hangs on the wall, silent and annotated.", null);
                }
            );
        }

    
    }
}
