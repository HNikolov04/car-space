import { useEffect, useState } from "react";
import {
  getCarServiceListingById,
  saveCarServiceListing,
  unsaveCarServiceListing,
  deleteCarServiceListing,
} from "../services/api/carServiceListingService";
import { getCurrentUser } from "../services/api/userService";

export default function useCarServiceDetails(id, navigate) {
  const [service, setService] = useState(null);
  const [isSaved, setIsSaved] = useState(false);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [data, user] = await Promise.all([
          getCarServiceListingById(id),
          getCurrentUser().catch(() => null),
        ]);
        setService(data);
        setIsSaved(data.isSavedByCurrentUser ?? false);
        setCurrentUserId(user?.id || null);
      } catch (err) {
        console.error(err);
        setError("Failed to load service.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleDelete = async () => {
    const confirmed = window.confirm("Are you sure you want to delete this service?");
    if (!confirmed) return;

    try {
      await deleteCarServiceListing(id);
      navigate("/car-service");
    } catch (err) {
      alert("Failed to delete. Try again.");
    }
  };

  const toggleSave = async () => {
    if (!currentUserId) {
      navigate("/login");
      return;
    }

    try {
      isSaved
        ? await unsaveCarServiceListing(id)
        : await saveCarServiceListing(id);
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Save toggle failed:", err);
    }
  };

  const isOwner = currentUserId === service?.userId;

  return {
    service,
    isSaved,
    isOwner,
    toggleSave,
    handleDelete,
    loading,
    error,
  };
}
