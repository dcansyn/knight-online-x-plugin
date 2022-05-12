using KO.Application.Boxes.Extensions;
using KO.Application.Characters.Extensions;
using KO.Application.Parties.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Boxes;
using KO.Domain.Characters;
using KO.Domain.Packets;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Addresses.Handlers
{
    public static class AddressActionHandler
    {
        public static async Task ReceiveAction(this Character character)
        {
            await character.ReadReceiveMessageAction(async (Packet packet) =>
            {
                switch (packet.Type)
                {
                    case PacketType.Chat:
                        /* 100E028E03088FBC8DE882AD82F157008B812991E58C95523588C88FE3206F72208377838B8375838C523588C88FE381408F6F2993578EA695A8202620836D834128332E35472982CC91678D8782B98EA88E6382B58388838D2888CA92753A3831322C35353529
                         * 100E021C0104834383942A008F6F816A8A4F93B996C28FF18292825481408B81816A8BAD8B7C81402888CA92753A3833342C35353229
                         */

                        /*If (msg_buffer.Substring(0, 4) = "1003" Or msg_buffer.Substring(0, 4) = "1002") Then ' Party chat
                            Dim charNameLen As Integer = FormatDec(msg_buffer.Substring(10, 2), 2)
                            Dim charName As String = HexToString(msg_buffer.Substring(12, charNameLen * 2))
                            Dim textLen As Integer = FormatDec(msg_buffer.Substring((12 + (charNameLen * 2)), 4), 4)
                            Dim textStart As Integer = (12 + (charNameLen * 2) + 4)
                            Dim textChat As String = HexToString(msg_buffer.Substring(textStart, msg_buffer.Length - textStart))

                            If (frm_2Genel.chk_chatSummon.Checked And
                                frm_2Genel.txt_chatSummon.Text <> "" And
                                textChat.IndexOf(frm_2Genel.txt_chatSummon.Text) >= 0 And
                                textChat.IndexOf(" ") >= frm_2Genel.txt_chatSummon.Text.Length And
                                getPartyCount() > 1 And getCharClassName() = "Mage") Then

                                If (textChat = frm_2Genel.txt_chatSummon.Text & " topla") Then
                                    SummonStart = True
                                    SummonRow = 1
                                    Exit Sub
                                ElseIf (textChat.Length = frm_2Genel.txt_chatSummon.Text.Length + 2) Then
                                    Dim chatNickRow As String = textChat.Split(" ")(1)
                                    If (PartyUserID(chatNickRow) <> getCharID()) Then
                                        summonUser(PartyUserID(chatNickRow))
                                        Exit Sub
                                    End If
                                End If

                            End If
                        End If*/
                        break;

                    case PacketType.Party:
                        if (packet.Code.Length < 4) return;

                        // 2F{partyType:2}
                        var partyType = packet.Code.Substring(2, 2);
                        switch (partyType)
                        {
                            case "02":
                                // 2F{partyType:2}CD030300{partyInviteName:nameLength}
                                if (Client.Party.AutoPartyAccept)
                                {
                                    var partyInviteName = packet.Code.Substring(12).ConvertHexToString();
                                    if (Client.Party.AutoPartyFriendList?.Contains(partyInviteName) == true)
                                        await character.SendPartyAccept();
                                }
                                break;
                        }

                        break;

                    case PacketType.StateChange:
                        if (packet.Code.Length < 10) return;

                        // 29{stateCharacterId:4}{stateType:2}{stateStatus:2}00000000000000
                        var stateCharacterId = packet.Code.Substring(2, 4).ConvertDwordToInt();
                        if (stateCharacterId == character.GetCharacterId())
                        {
                            var stateType = packet.Code.Substring(6, 2).ConvertDwordToInt();
                            var stateStatus = packet.Code.Substring(8, 2).ConvertDwordToInt();

                            if (stateType == 7 && stateStatus == 1) { } // Hiding Start
                            if (stateType == 7 && stateStatus == 2) { } // Hiding Start
                            if (stateType == 7 && stateStatus == 0) { } // Hiding End
                            if (stateType == 1 && stateStatus == 2) { } // Siting
                            if (stateType == 1 && stateStatus == 1) { } // Siting End

                            if ((stateType == 1 && stateStatus == 3) || (stateType == 3 && stateStatus == 4)) // Dead Starting / Completed
                                character.UpdateAvailableExpireTime();

                            if (stateType == 3 && stateStatus == 4) // Loading Start
                                character.UpdateAvailableExpireTime();

                            if (stateType == 3 && stateStatus == 7) // Loading Finish
                                character.UpdateAvailableExpireTime(0);
                        }
                        break;

                    case PacketType.Attack:
                        if (packet.Code.Length < 16) return;

                        // 080101{sourceId:4}{targetId:4}00
                        var sourceId = packet.Code.Substring(6, 4).ConvertDwordToInt();
                        var targetId = packet.Code.Substring(10, 4).ConvertDwordToInt();

                        if (character.Id == sourceId)
                        {
                            //
                        }
                        break;

                    case PacketType.Dead:
                        if (packet.Code.Length < 6) return;

                        // 11{deadId:4}00000000000000000000
                        var deadId = packet.Code.Substring(2, 4).ConvertDwordToInt();
                        if (deadId <= 0) return;

                        if (character.Id == deadId)
                        {
                            character.UpdateAvailableExpireTime();
                            return;
                        }

                        if (character.Target?.Id == deadId)
                            Client.CharacterTarget.UpdateTargetExpireTime();

                        if (!Client.DeadCharacters.Any(x => x.Id == deadId))
                            Client.DeadCharacters = Client.DeadCharacters.Append(new DeadCharacter(deadId)).ToArray();

                        await character.WriteLong(await character.GetTargetBase(deadId) + Settings.KO_OFF_TARGET_MOVE, 5);
                        break;

                    case PacketType.TargetHealth:
                        if (packet.Code.Length < 28) return;

                        // 22{healthTargetId:4}00{maxHealth:8}{health:8}0000
                        var healthTargetId = packet.Code.Substring(2, 4).ConvertDwordToInt();
                        var maxHealth = packet.Code.Substring(8, 8).ConvertDwordToInt();
                        var health = packet.Code.Substring(16, 8).ConvertDwordToInt();
                        if (healthTargetId <= 0 || character.Id == healthTargetId || health > 0) return;

                        if (character.Target?.Id == healthTargetId)
                            Client.CharacterTarget.UpdateTargetExpireTime();

                        if (!Client.DeadCharacters.Any(x => x.Id == healthTargetId))
                            Client.DeadCharacters = Client.DeadCharacters.Append(new DeadCharacter(healthTargetId)).ToArray();

                        await character.WriteLong(await character.GetTargetBase(healthTargetId) + Settings.KO_OFF_TARGET_MOVE, 5);
                        break;

                    case PacketType.ItemDrop:
                        if (packet.Code.Length < 16 || !character.IsBoxCollector) return;

                        // 23{dropTargetId:4}{dropBoxId:8}{dropItemStatus:2}
                        var dropTargetId = packet.Code.Substring(2, 4).ConvertDwordToInt();
                        var dropBoxId = packet.Code.Substring(6, 8).ConvertDwordToInt();
                        var dropItemStatus = packet.Code.Substring(14, 2).ConvertDwordToInt();

                        if (dropItemStatus <= 0 || dropBoxId <= 0 || dropItemStatus <= 0) return;

                        if (!Client.BoxCollect.IsGotoBox)
                        {
                            _ = character.Send("24", dropBoxId.ConvertToDword());
                            return;
                        }

                        var boxTarget = character.GetTargetById(dropTargetId, excludeDead: false);
                        if (boxTarget == null) return;

                        if (!Client.BoxLoots.Any(x => x.Id == dropBoxId))
                            Client.BoxLoots = Client.BoxLoots.Append(new Box(dropBoxId, dropTargetId, boxTarget.X, boxTarget.Y)).ToArray();

                        break;

                    case PacketType.BoxOpenReq:
                        if (packet.Code.Length < 156 || !character.IsBoxCollector) return;

                        // 24{openBoxId:8}{boxItemCount:2}[{openBoxItemId:8}{openBoxItemCount:4} * 12]
                        var openBoxId = packet.Code.Substring(2, 8).ConvertDwordToInt();

                        var openBoxIndex = 0;
                        for (int openBoxPacketIndex = 12; openBoxPacketIndex < 145; openBoxPacketIndex += 12)
                        {
                            var openBoxItemId = packet.Code.Substring(openBoxPacketIndex, 8).ConvertDwordToInt();
                            if (openBoxItemId > 0 && await character.IsCollectable(openBoxItemId))
                                _ = character.Send("26", openBoxId.ConvertToDword(), openBoxItemId.ConvertToDword(), openBoxIndex.ConvertToDword(1), "00");

                            openBoxIndex++;
                        }

                        Client.BoxLoots = Client.BoxLoots.Where(x => x.Id != openBoxId).ToArray();
                        break;

                    case PacketType.CollectBox:
                        if (packet.Code.Length < 34 || character.IsMain) return;

                        // 26{collectBoxCount:2}{collectBoxId:8}FF{collectBoxItemId:8}{collectBoxItemCount:4}{unknown:8}
                        var collectBoxCount = packet.Code.Substring(2, 2).ConvertDwordToInt();
                        var collectBoxId = packet.Code.Substring(4, 8).ConvertDwordToInt();
                        var collectBoxItemId = packet.Code.Substring(14, 8).ConvertDwordToInt();
                        var collectBoxItemCount = packet.Code.Substring(22, 4).ConvertDwordToInt();

                        // Money
                        if (collectBoxItemId == 900000000)
                        {

                        }
                        break;
                }
            });

            await Task.CompletedTask;
        }


        public static async Task SendAction(this Character character)
        {
            await character.ReadSendMessageAction((Packet packet) =>
            {
                switch (packet.Type)
                {
                    case PacketType.Gate:
                        character.UpdateAvailableExpireTime();
                        break;
                }
            });

            await Task.CompletedTask;
        }

        public static async Task ReadReceiveMessageAction(this Character character, Action<Packet> callback)
        {
            if (character.ReceiveHandle == IntPtr.Zero)
                throw new Exception("Receive handle not found.");

            byte[] packetBuffer;
            int packetSize;
            do
            {
                bool rc = WinApi.GetMailslotInfo(character.ReceiveHandle, IntPtr.Zero, out packetSize, out int messageCount, IntPtr.Zero);
                if (messageCount > 0)
                {
                    packetBuffer = new byte[packetSize];
                    if (rc)
                    {
                        _ = WinApi.ReadFile(character.ReceiveHandle, packetBuffer, (uint)packetSize, out uint fileRead, IntPtr.Zero);
                        if (fileRead != 0)
                        {
                            var packet = packetBuffer.ConvertByteArrayToHex();
                            var value = packetBuffer[0];
                            var type = PacketType.Unknown;
                            if (Enum.IsDefined(typeof(PacketType), value))
                                type = (PacketType)value;

                            callback(new Packet(packet, value, type, PacketStatusType.Receive));
                        }
                    }
                }
            } while (packetSize != -1);

            await Task.CompletedTask;
        }

        public static async Task ReadSendMessageAction(this Character character, Action<Packet> callback)
        {
            if (character.SendHandle == IntPtr.Zero)
                throw new Exception("Send handle not found.");

            byte[] packetBuffer;
            int packetSize;

            do
            {
                bool rc = WinApi.GetMailslotInfo(character.SendHandle, IntPtr.Zero, out packetSize, out int messageCount, IntPtr.Zero);
                if (messageCount > 0)
                {
                    packetBuffer = new byte[packetSize];
                    if (rc)
                    {
                        _ = WinApi.ReadFile(character.SendHandle, packetBuffer, (uint)packetSize, out uint fileRead, IntPtr.Zero);
                        if (fileRead != 0)
                        {
                            var packet = packetBuffer.ConvertByteArrayToHex();
                            var value = packetBuffer[0];
                            var type = PacketType.Unknown;
                            if (Enum.IsDefined(typeof(PacketType), value))
                                type = (PacketType)value;

                            callback(new Packet(packet, value, type, PacketStatusType.Send));
                        }
                    }
                }
            } while (packetSize != -1);

            await Task.CompletedTask;
        }
    }
}
