import { useEffect, useState } from "react";
import {
  getCarServiceListingById,
  updateCarServiceListing,
} from "../services/api/carServiceListingService";
import { getCarServiceCategories } from "../services/api/carServiceCategoryService";

export default function useEditCarServiceForm(id, navigate) {
  const [formData, setFormData] = useState(null);
  const [categories, setCategories] = useState([]);
  const [newImageFile, setNewImageFile] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getCarServiceListingById(id)
      .then(data => {
        setFormData(data);
        setLoading(false);
      })
      .catch(() => navigate("/car-service"));
  }, [id, navigate]);

  useEffect(() => {
    getCarServiceCategories()
      .then(setCategories)
      .catch(err => console.error("Failed to fetch categories:", err));
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    setNewImageFile(e.target.files[0]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      id,
      ...formData,
      price: parseFloat(formData.price),
      categoryId: parseInt(formData.categoryId),
      imageFile: newImageFile || null,
    };

    try {
      await updateCarServiceListing(payload);
      navigate(`/car-service/${id}`);
    } catch (err) {
      console.error("Update failed:", err.message);
    }
  };

  return {
    formData,
    categories,
    loading,
    handleChange,
    handleImageChange,
    handleSubmit,
  };
}
