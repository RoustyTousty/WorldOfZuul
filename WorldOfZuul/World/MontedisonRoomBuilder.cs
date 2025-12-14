using System.ComponentModel.Design;
using System.Data;
using WorldOfZuul.Entities;
using WorldOfZuul.Items;

namespace WorldOfZuul.World
{
    /*Builder for the Montedison corporate office – Milan, 1992.
     * This room contains interactive objects that reveal the company's hidden links
     * to shell corporations (Albatross Trading Ltd., Blue Horizon SA) and offshore transfers.
     * Includes nested interactions hidden.
     */
    public static class MontedisonRoomBuilder
    {
        public static Room BuildMontedisonOffice()
        {
            var room = new Room("Montedison", "Montedison Corporate Office","The office smells of old paper, cigarette smoke, and polished wood. "+
                "Documents lie scattered across a heavy oak desk, and a strange ticking echoes "+
                "from a wall clock that seems slightly off-beat. A large framed painting dominates the wall; "+
                "its placement feels too deliberate. Somewhere in this office, connections to shell companies "+
                "Albatross Trading Ltd. and Blue Horizon SA are hidden.\n"+
                "INTERACTIVE OBJECTS IN THIS ROOM:\n" +
                " • desk-Contains invoices and offshore payment notes.\n"+
                " • cigarette_case.\n"+
                " • filing_cabinet-LOCKED; requires Finance Cabinet Key.\n"+
                " • wall_clock.\n"+
                " • painting.\n"+
                " • safe-contains Swiss transfer orders.\n"+
                " • telephone-Contains a threatening message.\n"+
                " • usb_drive -Contains digital ledger (Albatross + Blue Horizon).\n");

                room.InteractiveObjects["desk"]= BuildDesk();
                room.InteractiveObjects["cigarette_case"]= BuildCigaratteCase();
                room.InteractiveObjects["filing_cabinet"] = BuildFilingCabinet();
                room.InteractiveObjects["wall_clock"] = BuildWallClock();
                room.InteractiveObjects["painting"] = BuildPainting();
                room.InteractiveObjects["safe"] = BuildSafe();
                room.InteractiveObjects["telephone"] = BuildTelephone();
                room.InteractiveObjects["usb_drive"] = BuildUsbDrive();

                room.Npcs["contini"]=BuildContiniNPC();

                return room;

        }

        //OBJECTS
        private static InteractiveObject BuildDesk()
        {
            return new InteractiveObject
            (
                "desk",
                "Office_Desk",
                "A heavy Office_Desk.",
                "Invoices and notes mentions Albatross Trading Ltd. and Zurich",
                (state,verb)=>
                {
                if (verb=="inspect")
                    return("You find invoices for consultancy fees routed offshore.", null);
                
                if(verb=="open drawer")
            {
                if (state.GetFlag("desk_opened"))
                    return("The drawer is empty.",null);
                    return("Inside the drawer is a note: 'Blue Horizon SA-code 314'.", s=> s.SetFlag("desk_opened"));
            }

                return("You can't do that with the desk.",null);

                }

            );
        }

        private static InteractiveObject BuildCigaratteCase()
        {
            return new InteractiveObject
            (
                "cigarette_case",
                "Cigarette Case",
                "A silver cigarette case",
                "Engraved inside ins a business card.",
                (state, verb) =>
                {
                    if (verb=="inspect"|| verb=="open case")
                        return("The card reads: R. Contini-CFO. On the back:'Zurich before audit.'",
                        null);
                    
                    return("Nothing happens",null);
                }
            );
        }

        private static InteractiveObject BuildWallClock()
        {
            return new InteractiveObject
            (
                "wall_clock",
                "Wall Clock",
                "An all wall clock.",
                "It ticks unevenly. Something clatters inside it.",
                (state,verb) =>
                {
                    if (verb=="inspect")
                    return("There is a loose panel behind the clock.",null);
                    if(verb=="open clock")
                    {
                        if(state.GetFlag("finance_key_taken"))
                            return("The clock is emty.",null);

                        return("Inside you find the Finance Cabinet Key.", s => s.SetFlag("finance_key_taken"));

                    }

                    return("Nothing happens.",null);
                }
            );
        }

        private static InteractiveObject BuildFilingCabinet()
        {
            return new InteractiveObject
            (
                "filing_cabinet",
                "Filing Cabinet",
                "A locked filing cabinet.",
                "The cabinet requires a key",
                (state, verb) =>
                {
                    if(verb=="inspect")
                        return("A keyhole labeled 'Finance'.",null);

                    if(verb=="open cabinet")
                    {
                        if(!state.GetFlag("finance_key_taken"))
                            return("You need the Finance Cabinet Key.", null);

                        return("Inside are contracts linking Montedison to Albatross Trading Ltd.",null);      
                    }

                    return("You can't do that.", null);
                }

            );
        }

        private static InteractiveObject BuildPainting()
        {
           return new InteractiveObject
           (
            "painting",
            "Corporate Painting",
            "A large corporate painting",
            "The frame is slightlyn loose.",
            (state,verb)=>
            {
                if(verb=="inspect")
                    return("The painting looks movable.",null);

                if(verb=="push painting")
                {
                    if(state.GetFlag("safe_revealed"))
                        return("The painting is already moved.",null);
                    
                    return("You reveal a hidden safe behind the painting.",s=> s.SetFlag("safe_revealed"));

                }

                return("Nothing happens.", null);
            }

           );
        }

        private static InteractiveObject BuildSafe()
        {
            return new InteractiveObject(
                "safe",
                "Hidden Safe",
                "A wall safe.",
                "It requires a numeric code.",
                (state, verb) =>
                {
                    if (!state.GetFlag("safe_revealed"))
                        return ("You see no safe here.", null);

                    if (verb == "open safe")
                        return (
                            "The safe contains Swiss transfer orders to Blue Horizon SA.",
                            null
                        );

                    return ("The safe remains closed.", null);
                }
            );
        }

        private static InteractiveObject BuildTelephone()
        {
            return new InteractiveObject(
                "telephone",
                "Telephone",
                "A rotary telephone.",
                "The receiver feels warm.",
                (state, verb) =>
                {
                    if (verb == "answer")
                        return (
                            "A voice whispers: 'You are being watched.'",
                            null
                        );

                    return ("The phone is silent.", null);
                }
            );
        }

        private static InteractiveObject BuildUsbDrive()
        {
            return new InteractiveObject(
                "usb_drive",
                "USB Drive",
                "A USB drive.",
                "It contains a digital ledger.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return (
                            "Ledger files connect Albatross Trading Ltd. and Blue Horizon SA.",
                            null
                        );

                    return ("Nothing happens.", null);
                }
            );
        }

        private static Npc BuildContiniNPC()
        {
            return new Npc
            (
                "contini",
                "R. Contini",
                "Montedison CFO",
                new List<DialogueNode>
                {
            new DialogueNode(
                "start",
                "These documents are restricted. What are you doing here?",
                new List<DialogueResponse>
                {
                    new DialogueResponse("Tell me about the offshore payments.", "offshore"),
                    new DialogueResponse("Who authorized the Zurich transfers?", "zurich")
                }
            ),
            new DialogueNode(
                "offshore",
                "They are legitimate consultancy fees.",
                new List<DialogueResponse>
                {
                    new DialogueResponse("Albatross Trading Ltd. is a shell company.", "caught")
                }
            ),
            new DialogueNode(
                "zurich",
                "Internal channels handled the transfers.",
                new List<DialogueResponse>
                {
                    new DialogueResponse("I found the transfer orders in the safe.", "caught")
                }
            ),
            new DialogueNode(
                "caught",
                "…You shouldn't have seen this.",
                new List<DialogueResponse>() 
            )
        }
                
            );
        }
    }
}