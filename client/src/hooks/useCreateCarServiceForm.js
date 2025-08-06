import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { getCarServiceCategories } from "../services/api/carServiceCategoryService";
import { createCarServiceListing } from "../services/api/carServiceListingService";
import { getToken } from "../services/api/httpClient";

export default function useCreateCarServiceForm() {
  const navigate = useNavigate();
  const { register, handleSubmit, reset } = useForm();
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    if (!getToken()) {
      navigate("/auth/login");
    }
  }, [navigate]);

  useEffect(() => {
    getCarServiceCategories()
      .then(setCategories)
      .catch((err) => console.error("Failed to fetch categories:", err.message));
  }, []);

  const onSubmit = async (data) => {
    try {
      const formData = {
        ...data,
        price: parseFloat(data.price),
        categoryId: parseInt(data.categoryId),
        imageFile: data.imageFile?.[0] || null,
      };

      const created = await createCarServiceListing(formData);
      reset();
      navigate(`/car-service/${created.id}`);
    } catch (err) {
      console.error("Failed to create service:", err.message);
    }
  };

  const navigateBack = () => navigate(-1);

  return {
    register,
    handleSubmit,
    onSubmit,
    categories,
    navigateBack,
  };
}
