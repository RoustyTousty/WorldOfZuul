using WorldOfZuul.Entities;
using WorldOfZuul.Items;
using System.Collections.Generic;

namespace WorldOfZuul.World
{
    /* Montedison Corporate Office – Milan, 1992 */
    public static class MontedisonRoomBuilder
    {
        public static Room BuildMontedisonOffice()
        {
            var room = new Room(
                "Montedison",
                "Montedison Corporate Office",
                "The office smells of old paper and polished wood. A heavy desk dominates the room.\n" +
                "A wall clock ticks unevenly. A filing cabinet stands against the wall.\n" +
                "A corporate painting hangs slightly crooked.\n\n" +
                "INTERACTIVE OBJECTS:\n" +
                " • desk\n" +
                " • drawer\n" +
                " • wall_clock\n" +
                " • filing_cabinet (locked)\n" +
                " • painting\n" +
                " • safe\n" +
                " • telephone\n" +
                " • usb_drive\n\n" +
                "Try commands like: inspect, open, push painting, answer telephone, view usb, talk"
            );

            room.InteractiveObjects["desk"] = BuildDesk();
            room.InteractiveObjects["drawer"] = BuildDrawer();
            room.InteractiveObjects["wall_clock"] = BuildWallClock();
            room.InteractiveObjects["filing_cabinet"] = BuildFilingCabinet();
            room.InteractiveObjects["painting"] = BuildPainting();
            room.InteractiveObjects["safe"] = BuildSafe();
            room.InteractiveObjects["telephone"] = BuildTelephone();
            room.InteractiveObjects["usb_drive"] = BuildUsbDrive();

            room.Npcs["contini"] = BuildContiniNPC();
            room.Exits["City"] = new Exit("City", "City", room);

            return room;
        }

        // ───────── DESK ─────────
        private static InteractiveObject BuildDesk()
        {
            return new InteractiveObject(
                "desk",
                "Desk",
                "A heavy oak desk.",
                "Invoices are scattered across it.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "inspect desk")
                    {
                        state.SetFlag("desk_inspected");
                        return ("You find invoices for consultancy fees routed offshore. One drawer seems loose.", null);
                    }

                    return ("Nothing else stands out.", null);
                }
            );
        }

        // ───────── DRAWER (SEPARATE OBJECT) ─────────
    private static InteractiveObject BuildDrawer()
{
    return new InteractiveObject(
        "drawer",
        "Desk Drawer",
        "A slightly loose drawer.",
        "It might contain something.",
        (state, verb) =>
        {
            // INSPECT / OPEN
            if (verb == "inspect" || verb == "inspect drawer" || verb == "open drawer")
            {
                if (state.GetFlag("drawer_opened"))
                    return ("The drawer is empty.", null);

                return (
                    "Inside the drawer is a handwritten note: 'Blue Horizon SA - code 314'.",
                    s =>
                    {
                        s.SetFlag("drawer_opened");
                        s.SetFlag("know_safe_code");
                    }
                );
            }

            return ("Nothing happens.", null);
        }
    );
}


        // ───────── WALL CLOCK ─────────
        private static InteractiveObject BuildWallClock()
        {
            return new InteractiveObject(
                "wall_clock",
                "Wall Clock",
                "An old wall clock.",
                "It ticks unevenly.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "inspect clock")
                        return ("There is a loose panel behind the clock.", null);

                    if (verb == "open clock")
                    {
                        if (state.GetFlag("finance_key_taken"))
                            return ("The clock is empty.", null);

                        return (
                            "Inside you find the Finance Cabinet Key.",
                            s => s.SetFlag("finance_key_taken")
                        );
                    }

                    return ("Nothing happens.", null);
                }
            );
        }

        // ───────── FILING CABINET ─────────
        private static InteractiveObject BuildFilingCabinet()
        {
            return new InteractiveObject(
                "filing_cabinet",
                "Filing Cabinet",
                "A locked filing cabinet.",
                "The cabinet requires a key.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "inspect cabinet")
                        return ("A keyhole labeled 'Finance'.", null);

                    if (verb == "open cabinet")
                    {
                        if (!state.GetFlag("finance_key_taken"))
                            return ("You need the Finance Cabinet Key.", null);

                        return (
@"MONTEDISON S.p.A. - INTERNAL CONSULTANCY AGREEMENT
Department: Finance
Date: 05/02/1992

Counterparty: Albatross Trading Ltd.
Registered Office: Nassau, Bahamas

Scope of Services:
Strategic advisory services related to international energy logistics.

Payment Structure:
Fixed monthly consultancy fee - CHF 400,000
Payments routed via Swiss correspondent accounts.

Notes:
Several clauses reference offshore confidentiality.
Signatures appear duplicated across multiple agreements.",
                            s => s.SetFlag("albatross_contracts_found")
                        );
                    }

                    return ("You can't do that.", null);
                }
            );
        }

        // ───────── PAINTING ─────────
        private static InteractiveObject BuildPainting()
        {
            return new InteractiveObject(
                "painting",
                "Corporate Painting",
                "A large corporate painting.",
                "The frame looks loose.",
                (state, verb) =>
                {
                    if (verb == "inspect")
                        return ("The painting seems movable.", null);

                    if (verb == "push painting" || verb == "move painting")
                    {
                        if (state.GetFlag("safe_revealed"))
                            return ("The painting has already been moved.", null);

                        return ("You reveal a hidden safe behind the painting.", s => s.SetFlag("safe_revealed"));
                    }

                    return ("Nothing happens.", null);
                }
            );
        }

        // ───────── SAFE ─────────
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
@"MONTEDISON -INTERNAL TRANSFER ORDER
Date: 12/03/1992

Recipient: Blue Horizon SA (Zurich)
Amount: CHF 4,800,000 -Consultancy Services
Authorization: R. Contini - CFO

The signature stands out.
Someone in this office can explain this.",
                            null
                        );

                    return ("The safe remains closed.", null);
                }
            );
        }

        // ───────── TELEPHONE (INFO ONLY) ─────────
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
@"The telephone rings.

When you answer, a tense voice speaks immediately:

'Contini, listen carefully.
The auditors are getting close.

The paper copies stay locked in Finance.
The real ledger isn't on paper anymore.
It was moved to something small. Portable.
Off the books.

If something happens, destroy this line.'

The call disconnects.",
                            s => s.SetFlag("usb_hint_received")
                        );

                    return ("Nothing happens.", null);
                }
            );
        }

        // ───────── USB DRIVE ─────────
        private static InteractiveObject BuildUsbDrive()
        {
            return new InteractiveObject(
                "usb_drive",
                "USB Drive",
                "A USB drive.",
                "It contains a digital ledger.",
                (state, verb) =>
                {
                    if (verb == "inspect" || verb == "view usb")
                        return (
@"FILE: OFFSHORE_LEDGER_92.dat

Multiple transaction logs detected.

Accounts under 'Albatross Trading Ltd.' and 'Blue Horizon SA'
are mirrored across the same Swiss clearing numbers.

Transfer dates align with internal Montedison approvals.
Authorization metadata traces back to a single executive account.

Several entries reference handwritten approval codes.
Paper verification is advised.",
                            null
                        );

                    return ("Nothing happens.", null);
                }
            );
        }

        // ───────── CONTINI NPC ─────────

       private static Npc BuildContiniNPC()
{
    return new Npc
    (
        "contini",
        "R. Contini",
        "Montedison CFO",
        new List<DialogueNode>
        {
            // ─────────────────────────────
            // START
            // ─────────────────────────────
            new DialogueNode(
                "start",
                "These offices are restricted. What exactly are you looking for?",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "I'm just reviewing internal financial documents.",
                        "neutral"
                    ),
                    new DialogueResponse(
                        "I have questions about offshore payments.",
                        "suspicious"
                    ),
                    new DialogueResponse(
                        "Why does Blue Horizon SA appear in your transfer orders?",
                        "direct"
                    )
                }
            ),

            // ─────────────────────────────
            // NEUTRAL PATH
            // ─────────────────────────────
            new DialogueNode(
                "neutral",
                "Then you already know these matters are confidential. Everything follows corporate protocol.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "Understood. I won't take much of your time.",
                        "neutral_end"
                    ),
                    new DialogueResponse(
                        "Can you explain the consultancy fees?",
                        "suspicious"
                    )
                }
            ),

            new DialogueNode(
                "neutral_end",
                "Good. Discretion is expected at this level.",
                new List<DialogueResponse>()
            ),

            // ─────────────────────────────
            // SUSPICIOUS PATH
            // ─────────────────────────────
            new DialogueNode(
                "suspicious",
                "Consultancy fees are standard practice. International operations require flexibility.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "Albatross Trading Ltd. doesn't seem operational.",
                        "pressure"
                    ),
                    new DialogueResponse(
                        "I see. Thank you for clarifying.",
                        "neutral_end"
                    )
                }
            ),

            new DialogueNode(
                "pressure",
                "You should be careful drawing conclusions from incomplete data.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "I found handwritten approval codes.",
                        "evidence_light"
                    ),
                    new DialogueResponse(
                        "You're right. I'll leave it there.",
                        "neutral_end"
                    )
                }
            ),

            // ─────────────────────────────
            // DIRECT CONFRONTATION
            // ─────────────────────────────
            new DialogueNode(
                "direct",
                "…Where did you hear that name?",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "It's written in the safe behind the painting.",
                        "evidence_heavy"
                    ),
                    new DialogueResponse(
                        "I saw it referenced in internal notes.",
                        "evidence_light"
                    )
                }
            ),

            // ─────────────────────────────
            // LIGHT EVIDENCE
            // ─────────────────────────────
            new DialogueNode(
                "evidence_light",
                "You're misinterpreting internal shorthand. That happens often.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "The USB ledger confirms the transfers.",
                        "evidence_heavy"
                    ),
                    new DialogueResponse(
                        "Then explain the Swiss clearing numbers.",
                        "pressure_final"
                    )
                }
            ),

            // ─────────────────────────────
            // HEAVY EVIDENCE
            // ─────────────────────────────
            new DialogueNode(
                "evidence_heavy",
                "You shouldn't have accessed those materials.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "Then you should explain them.",
                        "collapse"
                    ),
                    new DialogueResponse(
                        "I think the auditors will be interested.",
                        "collapse"
                    )
                }
            ),

            // ─────────────────────────────
            // FINAL PRESSURE
            // ─────────────────────────────
            new DialogueNode(
                "pressure_final",
                "This conversation is becoming inappropriate.",
                new List<DialogueResponse>
                {
                    new DialogueResponse(
                        "So is hiding financial routes.",
                        "collapse"
                    )
                }
            ),

            // ─────────────────────────────
            // COLLAPSE / END
            // ─────────────────────────────
            new DialogueNode(
                "collapse",
                "You have no idea how many people are involved in this. Leave. Now.",
                new List<DialogueResponse>()
            )
        }
    );
}
} 
}