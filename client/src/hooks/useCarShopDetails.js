import { useEffect, useState } from "react";
import {
  getCarShopListingById,
  deleteCarShopListing,
  saveCarShopListing,
  unsaveCarShopListing,
} from "../services/api/carShopListingService";
import { useAuth } from "../contexts/AuthContext";

export default function useCarShopDetails(id, navigate) {
  const [car, setCar] = useState(null);
  const [isSaved, setIsSaved] = useState(false);
  const [loading, setLoading] = useState(true);
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    const fetchCar = async () => {
      try {
        const data = await getCarShopListingById(id);
        setCar(data);
        setIsSaved(data.isSavedByCurrentUser || false);
      } catch (err) {
        console.error("Failed to fetch car listing:", err);
        navigate("/car-shop");
      } finally {
        setLoading(false);
      }
    };

    fetchCar();
  }, [id, navigate]);

  const handleDelete = async () => {
    if (!window.confirm("Are you sure you want to delete this listing?")) return;

    try {
      await deleteCarShopListing(id);
      navigate("/car-shop");
    } catch (err) {
      console.error("Failed to delete listing:", err);
    }
  };

  const toggleSave = async () => {
    if (!isAuthenticated) {
      return navigate("/login");
    }

    try {
      if (isSaved) {
        await unsaveCarShopListing(id);
      } else {
        await saveCarShopListing(id);
      }
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Failed to toggle save:", err);
    }
  };

  return {
    car,
    isSaved,
    loading,
    toggleSave,
    handleDelete,
  };
}
