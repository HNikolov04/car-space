import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import { useNavigate } from "react-router-dom";
import { useCarForumEdit } from "../../hooks/useCarForumEdit";

export default function CarForumEditPage() {
  const {
    formData,
    brands,
    error,
    handleChange,
    handleSubmit,
  } = useCarForumEdit();

  const navigate = useNavigate();

  if (!formData) return <div className="text-center py-10">Loading...</div>;

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader />

      <main className="flex-1 px-4 py-10 flex justify-center">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white border border-gray-300 shadow-md rounded-xl p-6 space-y-6"
        >
          <h1 className="text-2xl font-bold text-gray-800 mb-4">
            Edit Forum Article
          </h1>

          {error && (
            <div className="text-red-600 text-sm text-center font-medium">{error}</div>
          )}

          <div>
            <label htmlFor="title" className="block text-sm font-medium mb-1">
              Title
            </label>
            <input
              id="title"
              name="title"
              value={formData.title}
              onChange={handleChange}
              className="w-full border rounded-md px-3 py-2"
              required
            />
          </div>

          <div>
            <label htmlFor="description" className="block text-sm font-medium mb-1">
              Description
            </label>
            <textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              rows={5}
              className="w-full border rounded-md px-3 py-2"
              required
            />
          </div>

          <div>
            <label htmlFor="brandId" className="block text-sm font-medium mb-1">
              Brand
            </label>
            <select
              id="brandId"
              name="brandId"
              value={formData.brandId}
              onChange={handleChange}
              className="w-full border rounded-md px-3 py-2 cursor-pointer"
              required
            >
              <option value="">Select brand</option>
              {brands.map((brand) => (
                <option key={brand.id} value={brand.id}>
                  {brand.name}
                </option>
              ))}
            </select>
          </div>

          <div className="flex justify-between pt-4">
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
