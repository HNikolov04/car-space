import { useParams, useNavigate } from "react-router-dom";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import useEditCarServiceForm from "../../hooks/useEditCarServiceForm";

export default function CarServiceEditPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const {
    formData,
    categories,
    handleChange,
    handleImageChange,
    handleSubmit,
    loading,
  } = useEditCarServiceForm(id, navigate);

  if (loading || !formData) return <div className="text-center p-10">Loading...</div>;

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <CarHeader />

      <main className="flex-1 flex justify-center px-4 py-10">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8"
        >
          <p className="text-sm text-gray-500 mb-4">
            Updated at: {new Date(formData.updatedAt).toLocaleDateString()}
          </p>

          <label className="block text-sm font-semibold mb-2">Current Image</label>
          <div className="relative w-full h-64 rounded-md overflow-hidden group mb-4 border border-gray-200">
            <img
              src={
                formData.imageUrl
                  ? `${import.meta.env.VITE_API_BASE_URL}${formData.imageUrl}`
                  : "https://via.placeholder.com/600x300"
              }
              alt="Current"
              className="w-full h-full object-cover"
            />
            <div className="absolute inset-0 bg-black/40 opacity-0 group-hover:opacity-100 transition flex items-center justify-center cursor-pointer">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-8 w-8 text-white"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M15.232 5.232l3.536 3.536M9 13h3l9-9a1.414 1.414 0 00-2-2l-9 9v3z"
                />
              </svg>
              <input
                type="file"
                accept="image/*"
                onChange={handleImageChange}
                className="absolute inset-0 opacity-0 cursor-pointer"
              />
            </div>
          </div>

          <label className="block text-sm font-medium mb-1">Title</label>
          <input
            name="title"
            value={formData.title}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
          />

          <label className="block text-sm font-medium mb-1">Description</label>
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
          />

          <label className="block text-sm font-medium mb-1">Price</label>
          <input
            name="price"
            type="number"
            value={formData.price}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
          />

          <div className="flex gap-4 mb-4">
            <div className="flex-1">
              <label className="block text-sm font-medium mb-1">City</label>
              <input
                name="city"
                value={formData.city}
                onChange={handleChange}
                className="w-full border rounded-md px-3 py-2"
              />
            </div>
            <div className="flex-1">
              <label className="block text-sm font-medium mb-1">Address</label>
              <input
                name="address"
                value={formData.address}
                onChange={handleChange}
                className="w-full border rounded-md px-3 py-2"
              />
            </div>
          </div>

          <div className="flex gap-4 mb-6">
            <div className="flex-1">
              <label className="block text-sm font-medium mb-1">Phone Number</label>
              <input
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
                className="w-full border rounded-md px-3 py-2"
              />
            </div>
            <div className="flex-1">
              <label className="block text-sm font-medium mb-1">Category</label>
              <select
                name="categoryId"
                value={formData.categoryId}
                onChange={handleChange}
                className="w-full cursor-pointer border rounded-md px-3 py-2"
              >
                <option value="">Select category</option>
                {categories.map((cat) => (
                  <option key={cat.id} value={cat.id}>
                    {cat.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="flex justify-between">
            <button
              type="button"
              onClick={() => navigate(-1)}
              className="px-4 py-2 cursor-pointer bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Save Changes
            </button>
          </div>
        </form>
      </main>

      <Footer />
    </div>
  );
}
