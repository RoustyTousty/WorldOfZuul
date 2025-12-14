using WorldOfZuul.Items;
using WorldOfZuul.Entities;

namespace WorldOfZuul.World
{
    /*
     * Builder for Political Office 03 — Political Coordinator's Office, Italy, early 1990s (Mani Pulite era).
     *
     * NARRATIVE GOAL:
     * Create strongest suspicion of corruption but provide no concrete evidence.
     * All findings remain explainable within legal boundaries.
     * Final impression must be uncertainty.
     * Player cannot conclude guilt or innocence with confidence.
     *
     * TONE: Neutral, factual, observational. Short sentences. No metaphors, no moral judgments.
     *
     * Contains interactive objects: desk with three drawers, trash bin, filing cabinet, wall calendar.
     * NPC: Senior Assistant Moretti (calm, controlled, emphasizes collective decisions).
     */
    public static class Office3RoomBuilder
    {
        public static Room BuildPoliticalOffice03()
        {
            var room = new Room(
                "office_03",
                "Office 03 – Political Coordinator",
                "A municipal office on the fourth floor of Palazzo Comunale. The walls are painted dark cream. " +
                "Large windows overlook the central piazza. A heavy wooden desk occupies most of the room. " +
                "Tall filing cabinets stand against both side walls. A wall calendar hangs beside the door. " +
                "The furniture appears older and more substantial than in other offices. " +
                "Papers are stacked in organized piles. Everything is orderly but dense with documentation. " +
                "You notice a metal trash bin with a shredder beside it, rows of labeled files, and a calendar covered in entries.\n\n" +
                "INTERACTIVE OBJECTS IN THIS ROOM:\n" +
                " • desk - A heavy wooden desk with three drawers (try: inspect desk, open drawer left, open drawer right, open drawer center)\n" +
                " • trash - A metal trash bin beside a paper shredder (try: inspect trash, search trash)\n" +
                " • cabinet - Tall filing cabinets with political coordination files (try: inspect cabinet, open cabinet, search cabinet)\n" +
                " • calendar - A wall calendar densely marked with appointments (try: inspect calendar, read calendar)\n" +
                " • assistant - Senior Assistant Moretti manages the office (try: talk to moretti)"
            );

            // Add DESK with three drawers
            room.InteractiveObjects["desk"] = BuildDesk();

            // Add TRASH BIN with shredder
            room.InteractiveObjects["trash"] = BuildTrashBin();

            // Add FILING CABINET
            room.InteractiveObjects["cabinet"] = BuildFilingCabinet();

            // Add WALL CALENDAR
            room.InteractiveObjects["calendar"] = BuildWallCalendar();

            // Add NPC: Senior Assistant Moretti
            room.SetNpc(BuildSeniorAssistantMoretti());

            return room;
        }

        private static InteractiveObject BuildDesk()
        {
            return new InteractiveObject(
                "desk",
                "Wooden Desk",
                "A heavy wooden desk with three drawers.",
                "A dark wood desk with carved edges. Three drawers are visible. " +
                "The surface holds several closed folders, a telephone, and a leather appointment book. " +
                "A brass lamp sits at one corner. The center drawer has a lock.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A heavy wooden desk with three drawers. You notice:\n" +
                                " • Left drawer: slightly ajar, appears to contain folders\n" +
                                " • Right drawer: closed, no visible markings\n" +
                                " • Center drawer: locked, newer mechanism\n" +
                                "The desk appears well used and carefully maintained.", null);

                    // LEFT DRAWER: Political coordination files with indirect language
                    if (verb == "open drawer left" || verb == "open left drawer")
                    {
                        if (state.GetFlag("desk_left_opened"))
                            return ("The left drawer remains open. The political coordination files are still inside.", null);

                        return (
                            "You open the left drawer and examine the political coordination files. Each document is typed and formally structured.\n\n" +
                            " • File labeled 'Strategic Coordination Memo March 1992': Contains references to aligning legislative priorities with party leadership guidance. Language describes facilitating communication between council members and regional coordinators. No specific instructions given. Outcome described as collaborative consensus.\n" +
                            " • File labeled 'Influence Assessment Regional Development': Lists council members and their positions on infrastructure funding. Notes who is receptive to party direction. Uses phrases like anticipated cooperation and aligned interests. No explicit pressure documented.\n" +
                            " • File labeled 'Coalition Building Notes': Describes meetings between party representatives and independent councillors. References discussions of mutual benefit and shared objectives. Outcomes framed as voluntary agreements reached through dialogue.\n\n" +
                            "The documents describe coordination and influence. Language is formal but indirect. " +
                            "No explicit instructions appear. No illegal actions documented. " +
                            "Everything is framed as legitimate political activity within party structures.",
                            s => s.SetFlag("desk_left_opened")
                        );
                    }

                    // RIGHT DRAWER: Personal notes and correspondence with careful wording
                    if (verb == "open drawer right" || verb == "open right drawer")
                    {
                        if (state.GetFlag("desk_right_opened"))
                            return ("The right drawer is still open. The personal correspondence remains inside.", null);

                        return (
                            "You open the right drawer and find personal notes and correspondence. The writing is careful and measured.\n\n" +
                            " • Handwritten note dated 8 Feb: Reminder to follow up on discussions regarding contractor selection processes. Ensure party interests are considered within legal frameworks. Maintain documented procedures.\n" +
                            " • Letter from regional party office dated 15 Mar: References need for greater coordination on municipal contracts. Suggests regular consultation meetings. Emphasizes importance of documented decision making.\n" +
                            " • Draft response dated 22 Mar: Acknowledges coordination responsibilities. Confirms commitment to aligning local decisions with regional party priorities where legally permissible. Notes all actions will follow established procedures.\n" +
                            " • Personal memo dated 10 Apr: Lists upcoming discussions with council members. Describes desired outcomes without specifying methods. Uses phrases like encouraging favorable consideration and promoting alignment.\n\n" +
                            "The wording feels restrained and deliberate. Nothing promises exchanges or illegal actions. " +
                            "Everything references legal frameworks and documented procedures. " +
                            "The content suggests influence but provides no evidence of corruption.",
                            s => s.SetFlag("desk_right_opened")
                        );
                    }

                    // CENTER DRAWER: Personal financial records matching disclosures
                    if (verb == "open drawer center" || verb == "open center drawer")
                    {
                        if (state.GetFlag("desk_center_opened"))
                            return ("The center drawer is already open. Personal financial records are visible inside.", null);

                        if (!state.GetFlag("trash_searched") && !state.GetFlag("cabinet_searched"))
                            return ("The center drawer is locked. The mechanism does not respond. " +
                                    "You would need to explore the office more thoroughly before attempting to access it.\n" +
                                    "HINT: Examine other objects in the room first.", null);

                        return (
                            "You try the center drawer again. The lock opens. Inside are personal financial records.\n\n" +
                            "You find:\n" +
                            " • Bank statement showing salary of €3,200 per month as senior coordinator. Additional income listed as €2,400 from party consulting work with attached contract specifying advisory services on legislative strategy. Total matches declared income on financial disclosure form.\n" +
                            " • Receipts for dinners and meetings. Amounts range from €80 to €250. Each receipt lists attendees. All appear to be party officials or council members. Notes on receipts reference discussion topics like budget priorities and coalition strategy.\n" +
                            " • Investment account statement showing moderate savings accumulated over years. No large unexplained deposits. Growth consistent with declared income and standard returns.\n" +
                            " • Copy of annual financial disclosure form filed with municipal ethics office. All income sources documented. Party consulting work listed. Form stamped as reviewed.\n\n" +
                            "Income matches disclosed sources. Consulting work has documentation. Dinner expenses align with political coordination role. " +
                            "No unexplained wealth appears. Financial records show consistency with declared earnings. " +
                            "Everything can be explained within the scope of legitimate political work.",
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
                "Trash Bin with Shredder",
                "A metal trash bin beside a paper shredder.",
                "A standard metal trash bin positioned next to a small office shredder. " +
                "The shredder bin contains strips of paper. The trash bin holds crumpled documents.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "search trash" || verb == "look in trash")
                    {
                        if (state.GetFlag("trash_searched"))
                            return ("You have already searched the trash. Only shredded fragments and draft documents remain.", null);

                        return (
                            "You examine the trash bin and shredder carefully. Most material has been destroyed or discarded.\n\n" +
                            "You find:\n" +
                            " • Shredded paper strips in the shredder bin. You can make out partial words: coordination, discussed, committee, outcome, aligned. Nothing forms complete sentences. Context is impossible to reconstruct.\n" +
                            " • Crumpled draft memo with crossed out text. Visible portions reference scheduling conflicts and meeting logistics. Heavy editing obscures original content. Final version not present.\n" +
                            " • Torn envelope from regional party headquarters. Empty. No contents remain.\n" +
                            " • Discarded coffee cups and food wrappers from meetings.\n" +
                            " • Ball of crumpled notes with fragments visible: ensure, coordinate, priority, discuss further. No complete thoughts preserved.\n\n" +
                            "The shredded material prevents reconstruction. Fragments suggest political work but reveal nothing specific. " +
                            "Draft documents are incomplete and heavily edited. No intact correspondence exists. " +
                            "The destruction of documents raises questions but provides no answers.",
                            s => s.SetFlag("trash_searched")
                        );
                    }

                    return ("The trash bin sits beside the shredder, partially full.", null);
                }
            );
        }

        private static InteractiveObject BuildFilingCabinet()
        {
            return new InteractiveObject(
                "cabinet",
                "Filing Cabinets",
                "Tall filing cabinets with political coordination files.",
                "Two tall gray filing cabinets standing against the walls. Each drawer is labeled with categories like Party Coordination, Legislative Planning, Coalition Strategy. " +
                "The cabinets appear full and systematically organized.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("Filing cabinets containing political coordination documents. Labels include: " +
                                "'Party Strategy 1990 1991', 'Council Relations', 'Regional Coordination', 'Legislative Planning 1992'.\n" +
                                "The organization appears thorough and deliberate.", null);

                    if (verb == "open cabinet" || verb == "search cabinet")
                    {
                        if (state.GetFlag("cabinet_searched"))
                            return ("You have already examined the cabinets. The files remain generic and collectively focused.", null);

                        return (
                            "You open the filing cabinets and review the political coordination files. The documents are complete but carefully worded.\n\n" +
                            "You find:\n" +
                            " • Folder labeled 'Party Coordination Framework': Contains procedural guidelines for aligning municipal decisions with party positions. Describes consultation processes and communication protocols. Emphasizes collective decision making and documented procedures. No individual directives given.\n" +
                            " • Folder labeled 'Legislative Planning Sessions': Meeting minutes from party strategy sessions. Lists participants and discussion topics. Decisions described as consensus outcomes. Individual positions not attributed. Responsibility diffused across committees.\n" +
                            " • Folder labeled 'Council Member Profiles': Information on voting patterns and policy positions of all council members. Notes areas of alignment with party priorities. Suggests coordination opportunities. Does not document pressure or inducements.\n" +
                            " • Folder labeled 'Coalition Building Records': Correspondence regarding coalition agreements and policy negotiations. Language focuses on mutual interests and shared objectives. Outcomes framed as voluntary cooperation. No evidence of coercion or exchange.\n\n" +
                            "Files are complete and systematically maintained. Language is generic and collective. " +
                            "Decisions appear as group outcomes. Individual responsibility is unclear. " +
                            "Everything describes legitimate political coordination but feels carefully constructed.",
                            s => s.SetFlag("cabinet_searched")
                        );
                    }

                    return ("The filing cabinets remain closed, densely packed with documents.", null);
                }
            );
        }

        private static InteractiveObject BuildWallCalendar()
        {
            return new InteractiveObject(
                "calendar",
                "Wall Calendar",
                "A wall calendar densely marked with appointments.",
                "A wall calendar hanging beside the door. Nearly every day contains multiple entries. " +
                "The handwriting is small and cramped to fit the volume of appointments. Many entries are circled or starred.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "read calendar")
                    {
                        if (state.GetFlag("calendar_read"))
                            return ("You have already examined the calendar. The frequency of meetings and dinners remains notable.", null);

                        return (
                            "You examine the calendar closely. The density of entries is striking.\n\n" +
                            "Patterns you observe:\n" +
                            " • Dinner meetings occur three to four times per week. Labeled as informal discussion with party members, strategy session with regional coordinators, coalition planning dinner. Restaurants vary. No single location repeats frequently.\n" +
                            " • Office meetings scheduled nearly daily. Participants listed include council members, party officials, regional representatives. Topics noted as coordination, planning, consultation.\n" +
                            " • Evening appointments marked phone consultation or private discussion appear regularly. No locations given. No participant names listed.\n" +
                            " • Weekend entries include party committee sessions and strategy retreats.\n\n" +
                            "You cross reference the calendar with other documents in the office. " +
                            "Dinner dates align with major council votes documented in the filing cabinet. " +
                            "Strategy sessions precede legislative decisions by days or weeks. " +
                            "Phone consultations cluster around controversial budget approvals.\n\n" +
                            "The frequency suggests constant coordination. Timing correlates with political events. " +
                            "All entries can be explained as normal political work. None explicitly document illegal activity. " +
                            "The pattern raises suspicion but provides no proof.",
                            s => s.SetFlag("calendar_read")
                        );
                    }

                    return ("The calendar hangs near the door, covered with entries.", null);
                }
            );
        }

        private static Npc BuildSeniorAssistantMoretti()
        {
            var dialogueNodes = new List<DialogueNode>
            {
                new DialogueNode(
                    "greeting",
                    "Good afternoon. I am Senior Assistant Moretti. I coordinate administrative functions for this office. What information do you require?",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about the coordinator's role.", "role"),
                        new DialogueResponse("Ask about the frequency of meetings.", "meetings"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "role",
                    "The coordinator serves as liaison between municipal council and regional party leadership. " +
                    "The position ensures alignment of local policy with broader party strategy. " +
                    "This involves consultation, communication, and facilitating collective decision making. " +
                    "All activities operate within established political and legal frameworks.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("What does alignment mean?", "alignment"),
                        new DialogueResponse("Ask about the frequency of meetings.", "meetings"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "alignment",
                    "Alignment refers to coordinating municipal decisions with party priorities established through democratic processes. " +
                    "Party members are elected to represent constituents and advance party platforms. " +
                    "Coordination ensures policy consistency across levels of government. " +
                    "This is standard practice in representative democracy.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("How is coordination achieved?", "how_coordination"),
                        new DialogueResponse("Ask about the frequency of meetings.", "meetings"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "how_coordination",
                    "Through consultation meetings, strategy sessions, and regular communication. " +
                    "Council members receive guidance on party positions. Regional priorities are communicated. " +
                    "Discussions occur to identify areas of mutual interest and shared objectives. " +
                    "Final decisions remain with elected officials through formal voting procedures.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Is this guidance or pressure?", "pressure"),
                        new DialogueResponse("Ask about the frequency of meetings.", "meetings"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "pressure",
                    "Guidance. Party members voluntarily align with party positions as part of their political affiliation. " +
                    "No coercion occurs. Decisions are made collectively through established procedures. " +
                    "Council votes are public record. Individual members vote according to their judgment. " +
                    "Party coordination is a recognized aspect of democratic political systems.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about the frequency of meetings.", "meetings"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "meetings",
                    "The role requires extensive communication. Coordination meetings occur regularly. " +
                    "Strategy sessions with party leadership happen weekly. Consultations with council members are ongoing. " +
                    "Dinner meetings facilitate informal discussion in less formal settings. " +
                    "The frequency reflects the complexity of maintaining policy alignment across multiple stakeholders.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Why so many dinner meetings?", "dinners"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "dinners",
                    "Dinner meetings allow extended discussion in relaxed environments. " +
                    "Participants speak more freely outside formal office settings. " +
                    "Complex political negotiations benefit from informal atmosphere. " +
                    "This is common practice in political coordination work. All meetings are logged appropriately.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Are these meetings documented?", "documentation"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "documentation",
                    "Formal meetings are minuted. Informal consultations are noted in coordination records. " +
                    "Dinner meetings with official participants are logged. Financial disclosures include meal expenses. " +
                    "The level of documentation reflects the nature of each interaction. " +
                    "All documentation follows party and municipal guidelines.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("What about the shredded documents?", "shredding"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "shredding",
                    "Draft documents and working notes are routinely disposed of after final versions are completed. " +
                    "Shredding protects confidential political strategy discussions from premature disclosure. " +
                    "Final documents are retained according to record keeping requirements. " +
                    "This practice is standard in political offices handling sensitive coordination work.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("What is being hidden?", "hidden"),
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "hidden",
                    "Nothing is hidden. Working drafts and preliminary notes serve internal planning purposes. " +
                    "Final decisions and official communications are documented and retained. " +
                    "Political strategy discussions contain sensitive information about negotiating positions and internal deliberations. " +
                    "Protecting this material until decisions are finalized is appropriate and legal.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Ask about political coordination.", "coordination"),
                        new DialogueResponse("Thank you for your time.", "end"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "coordination",
                    "Political coordination ensures coherent policy implementation across party members holding elected positions. " +
                    "This involves communication, consultation, and collaborative decision making. " +
                    "All activities occur within legal and constitutional frameworks. " +
                    "Party structures facilitate democratic governance by organizing collective action around shared platforms.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Is any of this corrupt?", "corrupt"),
                        new DialogueResponse("Ask about the coordinator's role.", "role"),
                        new DialogueResponse("Leave.", "end")
                    }
                ),

                new DialogueNode(
                    "corrupt",
                    "No. Political coordination is a legitimate function of party systems in representative democracies. " +
                    "All documented activities comply with legal requirements. Financial disclosures are filed and reviewed. " +
                    "Decisions are made through established procedures with public voting records. " +
                    "Party coordination and corruption are distinct concepts. One involves organizing legitimate political activity. The other involves illegal exchanges.",
                    new List<DialogueResponse>
                    {
                        new DialogueResponse("Thank you for your time.", "end"),
                        new DialogueResponse("Leave.", "end")
                    }
                )
            };

            return new Npc(
                "assistant_moretti",
                "Senior Assistant Moretti",
                "A person in their forties wearing formal business attire. Posture is upright and controlled. " +
                "Speech is measured and precise. Eye contact is steady. Answers are calm and thorough. " +
                "Demeanor suggests experience in managing sensitive political work.",
                dialogueNodes
            );
        }
    }
}
