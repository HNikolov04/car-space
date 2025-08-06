import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { getCurrentUser } from "../services/api/userService";
import { createCarMeetListing } from "../services/api/carMeetListingService";

export default function useCarMeetCreate() {
  const { register, handleSubmit, reset } = useForm();
  const navigate = useNavigate();
  const [loadingUser, setLoadingUser] = useState(true);

  useEffect(() => {
    async function checkAuth() {
      try {
        await getCurrentUser();
        setLoadingUser(false);
      } catch {
        navigate("/login");
      }
    }
    checkAuth();
  }, [navigate]);

  const onSubmit = async (data) => {
    try {
      const formData = {
        ...data,
        imageFile: data.imageFile?.[0] || null,
      };

      const created = await createCarMeetListing(formData);
      reset();
      navigate(`/car-meet/${created.id}`);
    } catch (err) {
      console.error("Failed to create meet:", err);
      alert("Error creating meet. Please check your input.");
    }
  };

  const navigateBack = () => navigate(-1);

  return {
    register,
    handleSubmit,
    onSubmit,
    loadingUser,
    navigateBack,
  };
}
