using WorldOfZuul.Items;

namespace WorldOfZuul.World
{
    /*
     * Builder for the ENI Executive Office room — Milan, 1992.
     * Contains interactive objects (desk, painting, safe, phone, red folder, etc.)
     * with handlers for player actions like inspect, open, read, answer.
     */
    public static class ENIRoomBuilder
    {
        public static Room BuildENIExecutiveOffice()
        {
            
            var room = new Room(
                "ENI",
                "ENI Executive Office – Milan, 1992",
                "You stand inside a lavish yet oppressive office. The faint smell of cigars lingers in the air, " +
                "mixed with the dry scent of ink and old paper. A large mahogany desk dominates the room, covered with " +
                "folders, ledgers, and a golden fountain pen. Behind it, a massive abstract painting hides something unusual. " +
                "To your right stands a steel safe with a worn dial; to your left, a window overlooking Milan's business district. " +
                "A black rotary phone occasionally rings, breaking the silence. On top of the desk lies a red folder, placed too obviously to be ignored.\n\n" +
                "INTERACTIVE OBJECTS IN THIS ROOM:\n" +
                " • desk - A mahogany desk with drawers (try: inspect desk, open drawer left, open drawer right, open drawer center)\n" +
                " • folder - A red folder marked 'M.P.' (try: inspect folder, read folder, open folder, take folder)\n" +
                " • safe - A steel safe with a dial (try: inspect safe, open safe)\n" +
                " • painting - An abstract painting on the wall (try: inspect painting, push painting)\n" +
                " • phone - A black rotary phone (try: inspect phone, answer phone, answer)\n" +
                " • compartment - A secret compartment (revealed after pushing the painting)"
            );

            // Add DESK
            room.InteractiveObjects["desk"] = BuildDesk(room);

            // Add RED FOLDER
            room.InteractiveObjects["folder"] = BuildRedFolder();

            // Add SAFE
            room.InteractiveObjects["safe"] = BuildSafe(room);

            // Add PAINTING
            room.InteractiveObjects["painting"] = BuildPainting(room);

            // Add PHONE
            room.InteractiveObjects["phone"] = BuildPhone();

            // Add SECRET COMPARTMENT (revealed by painting)
            room.InteractiveObjects["compartment"] = BuildSecretCompartment();

            // Add actual Item objects so they can be taken with the normal "take" command
            var redFolder = new Item("folder", "A Red Folder (M.P.)", "A red folder marked 'M.P.' containing wire transfers and offshore banking evidence.");
            room.SetItem(redFolder);

            var briefcase = new Item("briefcase", "Leather Briefcase", "A leather briefcase stuffed with stacks of US dollars and confidential documents.");
            room.SetItem(briefcase);

            var ledger = new Item("ledger", "Bound Ledger (IOR)", "A leather-bound ledger labeled 'IOR channel — Zug (Switzerland)' listing offshore transactions.");
            room.SetItem(ledger);
            // Alias so commands like "take registry" work
            room.Items["registry"] = ledger;

            // Add exit back to City (TargetRoom will be set by Game.cs after all rooms are loaded)
            room.Exits["City"] = new Exit("City", "City", room); // Placeholder room, will be updated by Game.cs

            return room;
        }

        private static InteractiveObject BuildDesk(Room room)
        {
            return new InteractiveObject(
                "desk",
                "Mahogany Desk",
                "A heavy mahogany desk, lacquered to a mirror sheen.",
                "A heavy mahogany desk, lacquered to a mirror sheen. Three shallow drawers and a crimson folder splayed on top. " +
                "You notice the drawers are labeled left and right, with a center space that looks like it could open. " +
                "HINT: Try opening different drawers to find clues.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A heavy mahogany desk, lacquered to a mirror sheen. You notice:\n" +
                                " • A crimson folder on top (try: read folder)\n" +
                                " • Two shallow drawers on the sides (try: open drawer left, open drawer right)\n" +
                                " • A center compartment that might open (try: open drawer center)\n" +
                                "The desk itself looks sturdy and well-maintained.", null);

                    if (verb == "open drawer left" || verb == "open left drawer")
                    {
                        if (state.GetFlag("desk_left_opened"))
                            return ("The left drawer is now empty.... the code was 214", null);

                        return (
                            "You open the left drawer and find several receipts for inflated payments and a scrap of paper with circled digits: '214'.\n" +
                            "These receipts show payments for public contracts marked at much higher values than expected.",
                            s => s.SetFlag("desk_left_opened")
                        );
                    }

                    if (verb == "open drawer right" || verb == "open right drawer")
                    {
                        if (state.GetFlag("desk_right_opened"))
                            return ("The right drawer is now empty. The code was 742, but do you remember where it goes?", null);

                        return (
                            "You open the right drawer and find a leather-bound planner filled with entries.\n" +
                            "Key entries you find:\n" +
                            " • '16/3 — 09:00 — M.P. — 7-4-2' (7-4-2 looks like a combination!)\n" +
                            " • Several notes marked 'IOR' (Vatican Bank)\n" +
                            " • References to 'Zug' (Switzerland) and 'Caribbean accounts'\n" +
                            "HINT: That number sequence might be useful for opening something...",
                            s => s.SetFlag("desk_right_opened")
                        );
                    }

                    if (verb.StartsWith("open drawer center") || verb.StartsWith("open center drawer"))
                    {
                        if (state.GetFlag("desk_center_opened"))
                            return ("The center drawer is already open.", null);

                        if (!state.GetFlag("desk_right_opened"))
                            return ("The center drawer is locked. You need a code from somewhere...\n" +
                                    "HINT: Check the other drawers for clues.", null);

                        // Extract code from verb (e.g., "open drawer center 7-4-2" or "open drawer center 742")
                        string code = verb.Replace("open drawer center", "").Replace("open center drawer", "").Trim();
                        
                        if (string.IsNullOrEmpty(code))
                        {
                            return ("The center drawer is locked.\n"
                            + "TRY:open drawer center <code>.\n"
                            + "You found a code in one of the drawers, don't you remember? (TRY: inspect desk)", null);
                        }

                        // Accept both "7-4-2" and "742" formats
                        if (code == "7-4-2" || code == "742" || code == "7 4 2")
                        {
                            return (
                                "You carefully dial the combination 7-4-2. With a soft click, the center drawer swings open!\n" +
                                "Inside you find an envelope stamped 'Zug' (Switzerland) with several bank codes and account numbers. " +
                                "This confirms the connection between ENI and offshore banking.\n"+
                                "TRY: take envelope.",
                                s =>
                                {
                                    s.SetFlag("desk_center_opened");
                                    if (!room.Items.ContainsKey("envelope"))
                                    {
                                        room.SetItem(new Item(
                                            "envelope",
                                            "Envelope 'Zug' (Bank Codes)",
                                            "An envelope stamped 'Zug' containing several bank codes and account numbers linking ENI to offshore accounts."
                                        ));
                                    }
                                }
                            );
                        }
                        else
                        {
                            return ($"You try the code '{code}' but the drawer doesn't open. That's not the right combination.\n" +
                                    "HINT: The planner in the right drawer ", null);
                        }
                    }

                    return ("You can't do that with the desk.", null);
                }
            );
        }

        private static InteractiveObject BuildRedFolder()
        {
            return new InteractiveObject(
                "folder",
                "Red Folder",
                "A red folder marked 'M.P.' in block letters.",
                "A red folder marked 'M.P.' in block letters, placed conspicuously on the desk as if someone wanted it to be found.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "read folder" || verb == "open folder")
                    {
                        if (state.GetFlag("folder_read"))
                            return ("You've already read the folder. It contains wire transfers to shell companies and a suspicious note about offshore transfers.", null);

                        return (
                            "You carefully open the red folder marked 'M.P.' (Mani Pulite investigation?).\n" +
                            "Inside you find:\n" +
                            " • Copies of WIRE TRANSFERS to shell companies in Bahamas, Cayman Islands, and Switzerland\n" +
                            " • Handwritten note: 'Transfer offshore — IOR ensures coverage.' (IOR = Vatican Bank)\n" +
                            " • Margin note in different handwriting: 'Hide ledger; phone tonight 22:00.'\n" +
                            "This suggests someone is hiding evidence and planning to meet tonight.\n" +
                            "HINT: This contains damning evidence. You might want to take it (try: take folder).",
                            s => { s.SetFlag("folder_read"); s.SetFlag("player_has_evidence"); }
                        );
                    }

                    return ("You can't do that.", null);
                }
            );
        }

        private static InteractiveObject BuildSafe(Room room)
        {
            return new InteractiveObject(
                "safe",
                "Steel Safe",
                "A steel safe with a worn dial.",
                "A steel safe with a worn dial. The numbers around 2 and 4 show wear from repeated use. This is definitely locked.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A steel safe with a worn dial. Numbers around 2 and 4 show heavy wear. You need a combination to open it.\n" +
                                "HINT: Look for clues in the desk drawers, especially the planner.\n"+
                                "TRY: open safe <code>", null);

                    if (verb.StartsWith("open safe") || verb.StartsWith("unlock safe"))
                    {
                        if (!state.GetFlag("desk_left_opened"))
                            return ("You spin the dial, but it doesn't open. You need the combination from somewhere...\n" +
                                    "HINT: The receipts in the left desk drawer might have what you need.\n"+
                                    "TRY: open drawer left.", null);

                        if (state.GetFlag("safe_open"))
                            return ("The safe is already wide open.", null);

                        // Extract code from verb (e.g., "open safe 2-1-4")
                        string code = verb.Replace("open safe", "").Replace("unlock safe", "").Trim();
                        
                        if (string.IsNullOrEmpty(code))
                        {
                            return ("The safe is locked. You found circled digits on a receipt in the left drawer\n"+
                                    "TRY: open drawer left.", null);
                        }

                        // Accept both "2-1-4" and "214" formats
                        if (code == "2-1-4" || code == "214" || code == "2 1 4")
                        {
                            return (
                                "You carefully dial the combination 2-1-4. With a soft click, the safe swings open!\n" +
                                "Inside you find a bundle of evidence:\n" +
                                " • Classified ENI Documents (stamped CONFIDENTIAL)\n" +
                                " • Bank Statement - IOR Accounts (Caribbean transfers)\n" +
                                " • Sealed Photograph (suspicious handoff)\n" +
                                "These are treated as one set of documents.\n" +
                                "HINT: try: take documents, take statement or take photograph",
                                s => {
                                    s.SetFlag("safe_open");
                                    // Create a single pickable item representing the whole bundle
                                    if (!room.Items.ContainsKey("documents"))
                                    {
                                        var evidence = new Item(
                                            "documents",
                                            "Classified ENI Documents",
                                            "A bundle containing: Classified ENI Documents, Bank Statement - IOR Accounts, and a Sealed Photograph."
                                        );
                                        room.SetItem(evidence);
                                        // Aliases so any of these verbs pick the same bundle
                                        room.Items["statement"] = evidence;
                                        room.Items["photograph"] = evidence;
                                    }
                                }
                            );
                        }
                        else
                        {
                            return ($"You try the code '{code}' but the safe doesn't open. That's not the right combination.\n" +
                                    "HINT: The receipt in the left drawer showed: 2-1-4", null);
                        }
                    }

                    return ("The safe remains locked.", null);
                }
            );
        }

        private static InteractiveObject BuildPainting(Room room)
        {
            return new InteractiveObject(
                "painting",
                "Abstract Painting",
                "A massive abstraction of black and gold swirls.",
                "A massive abstraction of black and gold swirls. Something about it seems... off. The canvas appears to sit slightly away from the frame.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A massive abstraction of black and gold swirls. Up close, you notice a small seam on the right side of the frame. " +
                                "The canvas doesn't sit flush against the wall behind it.\n" +
                                "HINT: This painting might hide something. Try pushing or pulling it.\n"+
                                "TRY: push painting", null);

                    if (verb == "push painting" || verb == "pull frame" || verb == "push seam" || verb == "push")
                    {
                        if (state.GetFlag("painting_switch_found"))
                            return ("The hidden compartment is already open behind the painting.", null);

                        return (
                            "You push the seam and hear a soft CLICK. The painting slides aside on a hidden mechanism, " +
                            "revealing a narrow compartment carved into the wall.\n" +
                            "Inside the compartment you can see a LEATHER BRIEFCASE and a BOUND LEDGER.\n" +
                            "HINT: Try to inspect or take items from the compartment (try: inspect compartment).",
                            s => {
                                s.SetFlag("painting_switch_found");
                                s.SetFlag("secret_compartment_revealed");
                                // Combine compartment items into a single pickable bundle with aliases
                                var evidence = new Item(
                                    "evidence",
                                    "Compartment Evidence (Briefcase + Ledger)",
                                    "A combined set: a leather briefcase stuffed with US dollars and a bound ledger (IOR) with offshore transactions."
                                );
                                room.Items["evidence"] = evidence;
                                room.Items["briefcase"] = evidence;
                                room.Items["ledger"] = evidence;
                                room.Items["registry"] = evidence;
                            }
                        );
                    }

                    return ("Nothing happens.", null);
                }
            );
        }

        private static InteractiveObject BuildPhone()
        {
            return new InteractiveObject(
                "phone",
                "Black Rotary Phone",
                "A black rotary phone, its earpiece warm as if recently used.",
                "A black rotary phone. The earpiece feels warm, suggesting it was used recently. The cord is coiled neatly beside it. " +
                "It could ring at any moment with important information.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("A black rotary phone. The earpiece is warm. It could ring at any moment.\n" +
                                "HINT: Try answering the phone (try: answer).", null);

                    if (verb == "answer phone" || verb == "answer")
                    {
                        if (state.GetFlag("player_has_evidence"))
                            return (
                                "You pick up the phone. A warped, distorted voice crackles over the line:\n" +
                                "'You found things you shouldn't. Close them. Or close your eyes.'\n" +
                                "The line goes dead. Whoever this is knows you've been snooping and is threatening you.",
                                s => s.SetFlag("player_compromised")
                            );

                        return (
                            "You pick up the phone. A distorted voice warns:\n" +
                            "'The Vatican accounts must never surface. Do you understand?'\n" +
                            "The line goes dead. Your hands are shaking.",
                            null
                        );
                    }

                    return ("The phone is silent... for now.", null);
                }
            );
        }

        private static InteractiveObject BuildSecretCompartment()
        {
            return new InteractiveObject(
                "compartment",
                "Secret Compartment",
                "A hidden compartment in the wall, revealed behind the painting.",
                "A narrow compartment carved into the wall. Inside you see a LEATHER BRIEFCASE filled with US dollars and a BOUND LEDGER marked 'IOR channel — Zug (Switzerland)'.",
                (state, verb) =>
                {
                    if (!state.GetFlag("secret_compartment_revealed"))
                        return ("There's no compartment here. The painting blocks your view of the wall.", null);

                    if (verb == "inspect" || verb == "look inside" || verb == "look at compartment")
                        return ("A leather briefcase stuffed with stacks of US dollars (thousands... maybe millions).\n" +
                                "A leather-bound registry labeled 'IOR channel — Zug (Switzerland)' with lists of offshore transactions.\n" +
                                "HINT: These are extremely valuable pieces of evidence. Try taking them (try: take briefcase or take registry).", null);

                    if (verb == "take briefcase" || verb == "take registry" || verb == "take ledger" || verb == "grab briefcase")
                    {
                        return (
                            "You carefully remove the briefcase and ledger from the compartment and place them in your bag.\n" +
                            "Your heart is pounding. If anyone discovers these are missing, they'll know someone was here.\n" +
                            "These documents link ENI, the Vatican Bank (IOR), and offshore accounts in Switzerland. This is THE evidence.",
                            s => s.SetFlag("high_evidence_taken")
                        );
                    }

                    return ("You can't do that.", null);
                }
            );
        }
    }
}
