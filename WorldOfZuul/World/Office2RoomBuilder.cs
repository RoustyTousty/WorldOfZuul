using WorldOfZuul.Items;
using WorldOfZuul.Entities;

namespace WorldOfZuul.World
{
    /*
     * Builder for Political Office 02 — Infrastructure Councillor's Office, Italy, early 1990s (Mani Pulite era).
     *
     * NARRATIVE GOAL:
     * Create initial suspicion of corruption, but provide no concrete evidence.
     * All findings have legitimate explanations.
     * Final impression: suspicion alone is not proof. This councillor appears to follow the law.
     *
     * TONE: Neutral, factual, observant. No emotional language, no metaphors, no moral judgments.
     *
     * Contains interactive objects: desk with three drawers, trash bin, filing cabinet, wall calendar.
     * NPC: Assistant Bianchi (professional, calm, direct).
     */
    public static class Office2RoomBuilder
    {
        public static Room BuildPoliticalOffice02()
        {
            var room = new Room(
                "office_02",
                "Office 02 – Infrastructure Councillor",
                "A municipal office on the second floor of Palazzo Comunale. The walls are painted light gray. " +
                "Two tall windows face the street. A metal desk occupies the center of the room. " +
                "Filing cabinets line one wall. A wall calendar hangs near the door. Overhead lights provide even illumination. " +
                "The space is tidy and organized. Nothing stands out as unusual. " +
                "You notice a metal trash bin beside the desk, folders stacked neatly on shelves, and a calendar marked with appointments.\n\n" +
                "INTERACTIVE OBJECTS IN THIS ROOM:\n" +
                " • desk - A standard metal desk with three drawers (try: inspect desk, open drawer left, open drawer right, open drawer center)\n" +
                " • trash - A metal trash bin beside the desk (try: inspect trash, search trash)\n" +
                " • cabinet - A tall filing cabinet with infrastructure project files (try: inspect cabinet, open cabinet, search cabinet)\n" +
                " • calendar - A wall calendar with appointments (try: inspect calendar, read calendar)\n" +
                " • assistant - Assistant Bianchi works at the outer desk (try: talk to bianchi)"
            );

            // Add DESK with three drawers
            room.InteractiveObjects["desk"] = BuildDesk();

            // Add TRASH BIN
            room.InteractiveObjects["trash"] = BuildTrashBin();

            // Add FILING CABINET
            room.InteractiveObjects["cabinet"] = BuildFilingCabinet();

            // Add WALL CALENDAR
            room.InteractiveObjects["calendar"] = BuildWallCalendar();

            // Add NPC: Assistant Bianchi
            room.SetNpc(BuildAssistantBianchi());

            return room;
        }

        private static InteractiveObject BuildDesk()
        {
            return new InteractiveObject(
                "desk",
                "Metal Desk",
                "A standard municipal desk with three drawers.",
                "A standard gray metal desk. Three drawers are visible: left, right, and center. " +
                "The surface holds a telephone, a desk calendar, and several closed folders. A family photograph sits near the lamp. " +
                "The center drawer has a lock that appears newer than the desk itself.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A standard municipal desk with three drawers. You notice:\n" +
                                " • Left drawer: partially open, contains folders\n" +
                                " • Right drawer: closed, unlabeled\n" +
                                " • Center drawer: locked with a newer mechanism\n" +
                                "The desk surface is organized and clear of clutter.", null);

                    // LEFT DRAWER: Infrastructure project files with documented delays
                    if (verb == "open drawer left" || verb == "open left drawer")
                    {
                        if (state.GetFlag("desk_left_opened"))
                            return ("The left drawer remains open. The infrastructure project files are still inside.", null);

                        return (
                            "You open the left drawer and examine the infrastructure project files. Each folder contains project documentation with official stamps and dates.\n\n" +
                            " • Via Roma Bridge Repair (Delayed 4 months): Attached memo from regional engineering office states structural assessment required additional time due to unexpected foundation issues. Approved by council vote 8 to 2.\n" +
                            " • Public Transit Expansion (Delayed 5 months): Environmental impact study mandated by national law. Delay documented with reference to Legislative Decree 152/2006. Three different contractors submitted bids.\n" +
                            " • Water Infrastructure Upgrade (Delayed 7 months): Budget reallocation required council approval. Meeting minutes attached showing debate and vote. Contractor selected through public tender process with five participants.\n\n" +
                            "Each delay has a documented cause. Different companies appear across the projects. " +
                            "Council votes are recorded. Public tender procedures are followed. " +
                            "Nothing here suggests preferential treatment or unexplained decisions.",
                            s => s.SetFlag("desk_left_opened")
                        );
                    }

                    // RIGHT DRAWER: Meeting notes with clear technical language
                    if (verb == "open drawer right" || verb == "open right drawer")
                    {
                        if (state.GetFlag("desk_right_opened"))
                            return ("The right drawer is still open. The meeting notes remain inside.", null);

                        return (
                            "You open the right drawer and find a notebook containing meeting records. The entries are dated and detailed.\n\n" +
                            " • 12 Feb: Meeting with Strade Sicure srl. Discussed Via Roma bridge timeline. Reviewed engineering report sections 4.2 and 4.3. Confirmed compliance with safety code article 28.\n" +
                            " • 18 Mar: Council session. Presented water infrastructure budget proposal. Addressed questions regarding cost overruns. Provided breakdown of material price increases documented in attachment C.\n" +
                            " • 5 Apr: Site visit to transit expansion zone. Accompanied by regional transportation inspector and two council members. Inspected proposed route. Noted concerns about traffic flow during construction phase.\n" +
                            " • 22 May: Dinner meeting with infrastructure committee members. Discussed upcoming projects for next fiscal year. Logged as official meeting. Receipts filed with administration office.\n\n" +
                            "The language is specific and technical. Meetings reference documents and regulations. " +
                            "No vague wording appears. Decisions are tied to procedures. Dinner meetings are officially logged. " +
                            "Everything appears transparent and recorded.",
                            s => s.SetFlag("desk_right_opened")
                        );
                    }

                    // CENTER DRAWER: Personal documents with matching income
                    if (verb == "open drawer center" || verb == "open center drawer")
                    {
                        if (state.GetFlag("desk_center_opened"))
                            return ("The center drawer is already open. Personal financial documents are visible inside.", null);

                        if (!state.GetFlag("trash_searched") && !state.GetFlag("cabinet_searched"))
                            return ("The center drawer is locked. The mechanism does not open. " +
                                    "You would need to explore the office further before attempting to access it.\n" +
                                    "HINT: Examine other objects in the room first.", null);

                        return (
                            "You try the center drawer again. This time the lock opens. Inside are personal financial documents.\n\n" +
                            "You find:\n" +
                            " • Bank statement showing monthly salary of €2,600. No additional unexplained deposits. One entry marked consulting fee for €1,200 with attached invoice from professional engineering association for technical review work performed outside office hours.\n" +
                            " • Receipts for dinners at modest restaurants. Amounts range from €45 to €120. Each receipt has a note on the back listing attendees and meeting purpose. All match entries in official meeting logs.\n" +
                            " • Property tax statement for a small apartment. Purchase price and mortgage details indicate modest means. No evidence of unexplained wealth.\n" +
                            " • Annual financial disclosure form filed with municipal ethics office. All income sources listed. Form signed and dated. Stamped as reviewed and approved.\n\n" +
                            "Income and expenses appear consistent with declared salary. Consulting work is documented and invoiced. " +
                            "Dinner expenses match official records. No unexplained deposits exist. " +
                            "The financial picture shows a civil servant living within ordinary means.",
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
                "A standard office trash bin. It appears to be emptied regularly. The contents are minimal.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "search trash" || verb == "look in trash")
                    {
                        if (state.GetFlag("trash_searched"))
                            return ("You have already searched the trash bin. It contains only ordinary office waste.", null);

                        return (
                            "You examine the contents of the trash bin. The items inside are typical office waste.\n\n" +
                            "You find:\n" +
                            " • Several crumpled draft memos with crossed out text and corrections\n" +
                            " • An empty espresso cup\n" +
                            " • A torn envelope from the municipal budget office\n" +
                            " • Discarded printouts of spreadsheet pages showing project cost calculations\n" +
                            " • A newspaper folded to the local news section\n\n" +
                            "No gift packaging. No luxury items. No hidden correspondence. " +
                            "The trash contains exactly what you would expect from a working office. " +
                            "Nothing here raises questions or suggests anything beyond routine administrative work.",
                            s => s.SetFlag("trash_searched")
                        );
                    }

                    return ("The trash bin sits beside the desk, half empty.", null);
                }
            );
        }

        private static InteractiveObject BuildFilingCabinet()
        {
            return new InteractiveObject(
                "cabinet",
                "Filing Cabinet",
                "A tall metal filing cabinet containing infrastructure project files.",
                "A gray metal filing cabinet with four drawers. Each drawer is labeled by year and project type. " +
                "The folders inside appear complete and properly organized.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A filing cabinet containing infrastructure project files. The labels are clear: " +
                                "'Bridge Maintenance 1990', 'Public Works 1991', 'Infrastructure Contracts 1992'.\n" +
                                "The drawers appear full and well maintained.", null);

                    if (verb == "open cabinet" || verb == "search cabinet")
                    {
                        if (state.GetFlag("cabinet_searched"))
                            return ("You have already examined the cabinet. The files remain complete and organized.", null);

                        return (
                            "You open the filing cabinet and review the contents. The folders are complete and systematically arranged.\n\n" +
                            "You find:\n" +
                            " • Folder labeled 'Via Roma Bridge Repair Tender Process': Contains all 73 pages as listed in the index. Five company bids included. Evaluation matrix showing scoring criteria. Selection committee notes. Winning bid clearly justified by technical specifications and price.\n" +
                            " • Folder labeled 'Public Transit Environmental Review': Complete environmental impact assessment. All required signatures present. Third party consultant reports attached. Public comment period documentation included. No missing sections.\n" +
                            " • Folder labeled 'Citizen Complaint Log': Detailed entries for each complaint received. Response letters attached. Resolution documentation present. Complaints addressed within standard timeframes. No gaps in the record.\n" +
                            " • Folder labeled 'Council Meeting Minutes': Complete set of minutes from infrastructure committee meetings. Attendance recorded. Votes documented. Decisions clearly stated with supporting rationale.\n\n" +
                            "No files are missing. No pages are removed. Tender processes show multiple bidders. " +
                            "Complaints are addressed and documented. Decisions reference specific criteria and regulations. " +
                            "The filing system appears complete, transparent, and properly maintained.",
                            s => s.SetFlag("cabinet_searched")
                        );
                    }

                    return ("The filing cabinet remains closed, organized and complete.", null);
                }
            );
        }

        private static InteractiveObject BuildWallCalendar()
        {
            return new InteractiveObject(
                "calendar",
                "Wall Calendar",
                "A wall calendar for 1992 with handwritten appointments.",
                "A wall calendar hanging near the door. The pages show appointments written in clear handwriting. " +
                "Some entries are circled or marked with asterisks.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "read calendar")
                    {
                        if (state.GetFlag("calendar_read"))
                            return ("You have already examined the calendar. The appointments are logged and correspond to official records.", null);

                        return (
                            "You examine the calendar entries. The appointments are clearly marked and detailed.\n\n" +
                            "Entries include:\n" +
                            " • Regular council meetings every second Tuesday at 15:00\n" +
                            " • Site inspection visits marked with locations and accompanying staff names\n" +
                            " • Dinner meetings with infrastructure committee: Jan 20, Feb 24, Mar 30, Apr 27. Each marked with asterisk and note 'Official logged meeting'.\n" +
                            " • Budget review sessions with finance office\n" +
                            " • Public consultation events with times and venues listed\n\n" +
                            "You compare the dinner meeting dates with other documents in the office. " +
                            "Each dinner appears in the official meeting notebook in the right desk drawer. " +
                            "Each has corresponding entries in the council meeting minutes in the filing cabinet. " +
                            "Receipts for these dinners are in the center drawer, with attendee lists matching official records.\n\n" +
                            "The calendar shows a schedule of documented official activities. " +
                            "Dinner meetings are not hidden. They are marked as official and logged properly. " +
                            "All entries correspond to records elsewhere in the office. Nothing appears concealed or irregular.",
                            s => s.SetFlag("calendar_read")
                        );
                    }

                    return ("The calendar hangs near the door, marked with routine appointments.", null);
                }
            );
        }

        private static Npc BuildAssistantBianchi()
        {
            var dialogueNodes = new List<DialogueNode>
            {
                new DialogueNode(
                    "greeting",
                    "Good morning. I am Assistant Bianchi. I have worked in this office for seven years. How may I help you?",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about the councillor's work.", "work"),
                        new DialogueResponse("Ask about project delays.", "delays"),
                        new DialogueResponse("Ask about financial oversight.", "oversight"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "work",
                    "The councillor manages infrastructure projects for the municipality. Bridges, roads, public transit, water systems. " +
                    "The work requires coordination with engineering firms, regional offices, and budget committees. " +
                    "All decisions follow established procedures. Council votes are required for major expenditures.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("How are decisions documented?", "documentation"),
                        new DialogueResponse("Ask about project delays.", "delays"),
                        new DialogueResponse("Ask about financial oversight.", "oversight"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "documentation",
                    "Every project generates documentation. Tender announcements, bid evaluations, contractor selections, budget approvals, progress reports. " +
                    "Council meetings are minuted. Votes are recorded. Public consultation periods are logged. " +
                    "The files are available for audit. Regional oversight offices review them periodically. " +
                    "The system is designed for transparency.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Has this office been audited?", "audit"),
                        new DialogueResponse("Ask about project delays.", "delays"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "audit",
                    "Yes. The regional audit office conducted a review last year. They examined project files, financial records, and procurement procedures. " +
                    "The audit report found all documentation in order. Recommendations were made regarding filing system improvements, which we implemented. " +
                    "No irregularities were identified. The report is public record.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about project delays.", "delays"),
                        new DialogueResponse("Ask about financial oversight.", "oversight"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "delays",
                    "Project delays occur for various reasons. Engineering assessments take time. Environmental reviews are required by law. " +
                    "Budget approvals require council votes. Weather affects construction schedules. Material costs fluctuate. " +
                    "Each delay in our files has a documented cause and an adjusted timeline approved by the council.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Are contractors always the same?", "contractors"),
                        new DialogueResponse("Ask about financial oversight.", "oversight"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "contractors",
                    "No. We use public tender procedures. Multiple companies bid on each project. " +
                    "Evaluation committees score bids based on technical capacity, timeline, and price. " +
                    "The selection criteria are published in the tender announcement. Losing bidders can request evaluation details. " +
                    "Different contractors win different projects. This is documented in the tender files.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about financial oversight.", "oversight"),
                        new DialogueResponse("What about complaints?", "complaints"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "oversight",
                    "The councillor files annual financial disclosure forms with the municipal ethics office. " +
                    "Income sources are listed. Assets are declared. The forms are reviewed and approved. " +
                    "Bank statements are provided when requested. The disclosure process is mandatory for all elected officials. " +
                    "Compliance is monitored.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Has there been any investigation?", "investigation"),
                        new DialogueResponse("What about complaints?", "complaints"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "investigation",
                    "Not of this office. The Mani Pulite investigations have focused on other municipalities and departments. " +
                    "We have cooperated with regional audits and provided all requested documentation. " +
                    "No concerns have been raised about this office. The councillor has not been questioned or charged with any violation. " +
                    "Our procedures are consistent with legal requirements.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("What about complaints?", "complaints"),
                        new DialogueResponse("Thank you for your time.", "end"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "complaints",
                    "Citizens file complaints when projects are delayed or when they disagree with decisions. " +
                    "We maintain a complaint log. Each complaint receives a written response. Explanations are provided. " +
                    "Some complaints result in project modifications. Others are explained and closed. " +
                    "The log is complete and available for review. This is part of our accountability process.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Do complaints lead to changes?", "changes"),
                        new DialogueResponse("Ask about the councillor's work.", "work"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "changes",
                    "Sometimes. If a complaint identifies a valid concern, we address it. " +
                    "A complaint about traffic disruption led to revised construction schedules. " +
                    "A complaint about accessibility led to design modifications for a bridge project. " +
                    "Not all complaints result in changes, but all receive consideration and response. " +
                    "This is documented in the files.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about the councillor's work.", "work"),
                        new DialogueResponse("Thank you for your time.", "end"),
                        new DialogueResponse("Leave.", "end")
                    }
                )
            };

            return new Npc(
                "assistant_bianchi",
                "Assistant Bianchi",
                "A person in their thirties wearing business attire. Hair neat, posture upright, expression neutral. " +
                "Speaks clearly and directly. Answers questions without hesitation. Shows no signs of discomfort or evasion. " +
                "Appears professional and organized.",
                dialogueNodes
            );
        }
    }
}
