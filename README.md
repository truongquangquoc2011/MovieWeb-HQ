# 🎬 MovieWeb-HQ

> Website xem phim trực tuyến với giao diện hiện đại, hỗ trợ tìm kiếm, phân loại và xem phim chất lượng cao. Dự án được phát triển bằng ASP.NET dành cho sinh viên học lập trình web và thực hành xây dựng hệ thống giải trí thực tế.

## 🧩 Giới thiệu

**MovieWeb-HQ** là một dự án website cho phép người dùng xem phim trực tuyến với các tính năng cơ bản như tìm kiếm, lọc phim, hiển thị trailer, chi tiết phim và chuẩn bị cho các tính năng nâng cao như đánh giá và bình luận.

Dự án được viết bằng **C# ASP.NET**, phù hợp để triển khai như một bài tập lớn hoặc đồ án môn học trong lĩnh vực phát triển ứng dụng web.

## 🚀 Tính năng

- 📺 Xem danh sách các phim mới cập nhật
- 🔍 Tìm kiếm phim theo tên
- 🎭 Lọc phim theo thể loại, quốc gia, năm phát hành
- 🎞️ Trang chi tiết phim (mô tả, ảnh, trailer, diễn viên)
- 💬 Bình luận phim 
- 🎨 Giao diện responsive, tối ưu trải nghiệm người dùng
- 🧰 Quản lý phim qua giao diện admin (tùy chọn)

## 🛠️ Công nghệ sử dụng

| Công nghệ | Mô tả |
|----------|-------|
| **C# ASP.NET** | Viết backend xử lý logic và giao diện |
| **HTML/CSS/JS** | Tạo giao diện người dùng frontend |
| **SQL Server** | Lưu trữ dữ liệu phim |
| **Entity Framework** | ORM hỗ trợ thao tác dữ liệu dễ dàng |
| **Visual Studio** | IDE chính để phát triển và chạy dự án |

## 📁 Cấu trúc thư mục chính

MovieWeb-HQ/
├── Controllers/ # Các controller xử lý logic trang web
├── Models/ # Các lớp model ánh xạ dữ liệu
├── Views/ # Giao diện (HTML Razor)
├── wwwroot/ # File tĩnh (CSS, JS, ảnh)
├── MovieWeb-HQ.sln # File giải pháp mở bằng Visual Studio

## 🚀 Hướng dẫn cài đặt và chạy

### Bước 1: Clone về máy

```bash
git clone https://github.com/truongquangquoc2011/MovieWeb-HQ.git
cd MovieWeb-HQ
Bước 2: Mở bằng Visual Studio
Mở file MovieWeb-HQ.sln bằng Visual Studio 2022

Restore các package NuGet nếu được yêu cầu

Bước 3: Cấu hình cơ sở dữ liệu
Tạo một cơ sở dữ liệu SQL Server

Cập nhật chuỗi kết nối trong appsettings.json hoặc web.config

Bước 4: Chạy ứng dụng
Bấm nút Run (F5) trong Visual Studio

Truy cập: http://localhost:xxxx để chạy thử website

💡 Gợi ý phát triển thêm
✅ Hệ thống phân quyền: user, admin

✅ Tính năng đăng ký/đăng nhập

✅ Giao diện quản trị phim

✅ Tính năng xem trailer trực tiếp

✅ Thêm phần đánh giá (rating), bình luận (comment)

🤝 Đóng góp
Mọi đóng góp đều được hoan nghênh! Bạn có thể:

Fork dự án

Tạo nhánh mới: git checkout -b feature/tinh-nang-moi

Commit: git commit -m "Thêm tính năng ABC"

Push: git push origin feature/tinh-nang-moi

Tạo Pull Request để mình kiểm tra và hợp nhất

📄 License
Dự án được phát hành theo giấy phép MIT License – bạn có thể tự do sử dụng với mục đích học tập và phi thương mại.
