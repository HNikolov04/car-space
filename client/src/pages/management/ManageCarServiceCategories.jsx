import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import MainPageHeader from "../../components/shared/MainPageHeader";
import Footer from "../../components/shared/Footer";
import {
  getCarServiceCategories,
  createCarServiceCategory,
  updateCarServiceCategory,
  deleteCarServiceCategory,
} from "../../services/api/carServiceCategoryService";
import { useAuth } from "../../contexts/AuthContext";

export default function ManageCarServiceCategories() {
  const navigate = useNavigate();
  const { user } = useAuth();

  const [categories, setCategories] = useState([]);
  const [formName, setFormName] = useState("");
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);

  useEffect(() => {
    if (!user || user.role !== "Administrator") {
      navigate("/");
    }
  }, [user, navigate]);

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await getCarServiceCategories();
      setCategories(data || []);
    } catch {
      console.error("Failed to load categories");
    }
  };

  const handleSubmit = async () => {
    if (!formName.trim()) return;
    try {
      if (selectedCategoryId != null) {
        await updateCarServiceCategory({ id: selectedCategoryId, name: formName });
      } else {
        await createCarServiceCategory({ name: formName });
      }
      setFormName("");
      setSelectedCategoryId(null);
      await loadCategories();
    } catch (err) {
      console.error("Save failed:", err);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete this category?")) return;
    await deleteCarServiceCategory(id);
    await loadCategories();
  };

  const handleEdit = (cat) => {
    setFormName(cat.name);
    setSelectedCategoryId(cat.id);
  };

  const handleCancel = () => {
    setFormName("");
    setSelectedCategoryId(null);
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <MainPageHeader />
      <main className="flex-1 px-4 py-10 flex justify-center">
        <div className="max-w-3xl w-full bg-white shadow-lg rounded-xl border border-gray-300 p-8">
          <h2 className="text-2xl font-semibold mb-6 text-center">Manage Car Service Categories</h2>
          <div className="flex flex-col sm:flex-row gap-3 mb-6">
            <input
              value={formName}
              onChange={(e) => setFormName(e.target.value)}
              placeholder="Category Name"
              className="flex-1 border rounded-md px-3 py-2"
            />
            <div className="flex gap-2">
              <button onClick={handleSubmit} className="px-4 py-2 bg-blue-600 text-white rounded-md">{
                selectedCategoryId !== null ? "Update" : "Add"
              }</button>
              {selectedCategoryId !== null && (
                <button onClick={handleCancel} className="px-4 py-2 bg-red-600 text-white rounded-md">Cancel</button>
              )}
            </div>
          </div>
          <div className="overflow-x-auto">
            <table className="w-full text-sm border border-gray-300">
              <thead className="bg-gray-100 text-left">
                <tr>
                  <th className="border px-3 py-2 w-16">ID</th>
                  <th className="border px-3 py-2">Name</th>
                  <th className="border px-3 py-2 w-40">Actions</th>
                </tr>
              </thead>
              <tbody>
                {categories.map(cat => (
                  <tr key={cat.id} className="hover:bg-gray-50">
                    <td className="border px-3 py-2">{cat.id}</td>
                    <td className="border px-3 py-2">{cat.name}</td>
                    <td className="border px-3 py-2 space-x-2">
                      <button onClick={() => handleEdit(cat)} className="bg-blue-600 text-white px-3 py-1 rounded">Edit</button>
                      <button onClick={() => handleDelete(cat.id)} className="bg-red-600 text-white px-3 py-1 rounded">Delete</button>
                    </td>
                  </tr>
                ))}
                {categories.length === 0 && (
                  <tr>
                    <td colSpan="3" className="text-center text-gray-500 py-4 italic">
                      No categories available.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
          <div className="mt-8 text-center">
            <button onClick={() => navigate("/management")} className="px-4 py-2 bg-blue-600 text-white rounded-md">
              Back to Management
            </button>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
