import { useEffect, useState } from "react";
import {
  getCarMeetById,
  joinCarMeetListing,
  leaveCarMeetListing,
  saveCarMeetListing,
  unsaveCarMeetListing,
  deleteCarMeetListing,
  getCarMeetParticipants,
} from "../services/api/carMeetListingService";
import { getCurrentUser } from "../services/api/userService";

export default function useCarMeetDetails(id, navigate) {
  const [meet, setMeet] = useState(null);
  const [participants, setParticipants] = useState([]);
  const [joined, setJoined] = useState(false);
  const [saved, setSaved] = useState(false);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const [meetData, userData] = await Promise.all([
          getCarMeetById(id),
          getCurrentUser().catch(() => null),
        ]);

        setMeet(meetData);
        setSaved(meetData.isSavedByCurrentUser || false);
        setJoined(meetData.isJoinedByCurrentUser || false);
        setCurrentUserId(userData?.id || null);

        const participantData = await getCarMeetParticipants(id);
        setParticipants(participantData.items || []);
      } catch (err) {
        console.error("Failed to load meet:", err);
        navigate("/not-found");
      } finally {
        setLoading(false);
      }
    };

    fetchAll();
  }, [id, navigate]);

  const toggleSave = async () => {
    if (!currentUserId) return navigate("/login");
    try {
      saved ? await unsaveCarMeetListing(id) : await saveCarMeetListing(id);
      setSaved((prev) => !prev);
    } catch (err) {
      console.error("Toggle save failed:", err);
    }
  };

  const handleJoin = async () => {
    if (!currentUserId) return navigate("/login");
    try {
      await joinCarMeetListing(id);
      setJoined(true);
    } catch (err) {
      console.error("Join failed:", err);
    }
  };

  const handleLeave = async () => {
    try {
      await leaveCarMeetListing(id);
      setJoined(false);
    } catch (err) {
      console.error("Leave failed:", err);
    }
  };

  const handleDelete = async () => {
    const confirmed = window.confirm("Are you sure you want to delete this meet?");
    if (!confirmed) return;
    try {
      await deleteCarMeetListing(id);
      navigate("/car-meet");
    } catch (err) {
      console.error("Delete failed:", err);
    }
  };

  return {
    meet,
    participants,
    loading,
    joined,
    saved,
    isOwner: currentUserId === meet?.userId,
    isLoggedIn: !!currentUserId,
    toggleSave,
    handleJoin,
    handleLeave,
    handleDelete,
  };
}
