export default function Pagination({ currentPage, totalPages, onPageChange }) {
  const pages = Array.from({ length: totalPages }, (_, i) => i + 1);

  return (
    <div className="mt-8 flex justify-center gap-2">
      {pages.map((p) => (
        <button
          key={p}
          onClick={() => onPageChange(p)}
          className={`px-3 py-1 border rounded font-bold transition ${
            currentPage === p
              ? "bg-blue-600 cursor-default text-white"
              : "hover:bg-gray-200 cursor-pointer"
          }`}
        >
          {p}
        </button>
      ))}
    </div>
  );
}
