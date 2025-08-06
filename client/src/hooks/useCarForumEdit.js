import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getCarForumArticleById, updateCarForumArticle } from "../services/api/carForumArticleService";
import { getCarForumBrands } from "../services/api/carForumBrandService";
import { useAuth } from "../contexts/AuthContext";
import { ROUTES } from "../utils/routes/routes";

export function useCarForumEdit() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();

  const [formData, setFormData] = useState(null);
  const [brands, setBrands] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!isAuthenticated) {
      navigate(ROUTES.login);
      return;
    }

    const loadData = async () => {
      try {
        const [article, brandList] = await Promise.all([
          getCarForumArticleById(id),
          getCarForumBrands(),
        ]);

        if (article.userId !== user?.id) {
          navigate(ROUTES.carForum);
          return;
        }

        setFormData({
          id: article.id,
          title: article.title,
          description: article.description,
          brandId: article.brandId || "",
        });

        setBrands(brandList);
      } catch (err) {
        console.error("Failed to load article:", err);
        setError("Unable to load article.");
        navigate(ROUTES.carForum);
      }
    };

    loadData();
  }, [id, isAuthenticated, navigate, user?.id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const payload = {
        id: formData.id,
        title: formData.title,
        description: formData.description,
        brandId: parseInt(formData.brandId),
      };

      await updateCarForumArticle(payload);
      navigate(`/car-forum/${id}`);
    } catch (err) {
      console.error("Failed to update article:", err);
      setError("Failed to save changes.");
    }
  };

  return {
    formData,
    brands,
    error,
    handleChange,
    handleSubmit,
  };
}
