-- Thêm data phim chiếu trong tháng
USE CinemaDB; 
GO

INSERT INTO Movies (Title, Duration, Description)
VALUES 
(N'Ánh Dương Của Mẹ', 135, N'Toàn quốc'),
(N'Bẫy Tiến (Phim Việt Nam)', 113, N'Toàn quốc'),
(N'Dịch Vụ Giao Hàng Của Phù Thủy Kiki', 103, N'Toàn quốc'),
(N'Dưới Bóng Điện Hạ', 117, N'Toàn quốc'),
(N'Takhon: Quỷ Đội Lốt Người', 99, N'Toàn quốc'),
(N'Phí Phông: Quỷ Máu Rừng Thiêng', 120, N'Toàn quốc (Có suất chiếu sớm từ 16/04)'),
(N'Trùm Sò (Phim Việt Nam)', 120, N'Toàn quốc'),
(N'Đại Tiệc Trăng Máu 8 (Phim Việt Nam)', 120, N'Toàn quốc'),
(N'Mèo Siêu Quậy Ở Viện Bảo Tàng 2', 120, N'Toàn quốc'),
(N'Michael', 120, N'Toàn quốc');
PRINT N'ĐÃ THÊM THÀNH CÔNG 10 PHIM MỚI VÀO DATABASE!';


-- Thêm dữ liệu số phòng chiếu 
USE CinemaDB; -- Nhớ đổi thành tên database thực tế của bạn nếu khác nhé
GO
-- Reset ID về 0 để Rạp 1 bắt đầu từ ID = 1
DBCC CHECKIDENT ('Rooms', RESEED, 0);
GO
-- Thêm dữ liệu rạp phim
INSERT INTO Rooms (RoomName, TotalSeats)
VALUES 
(N'Rạp 1', 50),
(N'Rạp 2', 80),
(N'Rạp 3', 80),
(N'Rạp 4', 95),
(N'Rạp 5', 120),
(N'IMAX', 160),
(N'4DX', 160);

PRINT N'ĐÃ THÊM THÀNH CÔNG 7 RẠP PHIM MỚI!';
SELECT * FROM Rooms; -- Hiển thị lên để kiểm tra luôn


-- Thêm dữ liệu các suất chiếu phim
USE CinemaDB; -- Đổi thành tên database của bạn
GO

-- 1. DỌN SẠCH DỮ LIỆU CŨ ĐỂ TẠO LẠI BẢN MỚI
DELETE FROM Bookings;
DELETE FROM ShowTimes;
DBCC CHECKIDENT ('ShowTimes', RESEED, 0);

IF OBJECT_ID('Bookings', 'U') IS NOT NULL 
    DBCC CHECKIDENT ('Bookings', RESEED, 0);
GO

-- 2. KHAI BÁO BIẾN
DECLARE @StartDate DATE = '2026-04-01';
DECLARE @EndDate DATE = '2026-05-20';
DECLARE @CurrentDate DATE = @StartDate;
DECLARE @AdMinutes INT = 3; 
DECLARE @GapMinutes INT = 15; 

WHILE @CurrentDate <= @EndDate
BEGIN
    DECLARE @RoomID INT = 1;

    WHILE @RoomID <= 7
    BEGIN
        -- TẠO SỰ KHÁC BIỆT: Giờ chiếu đầu tiên trong ngày sẽ xê dịch 0, 10, 20 hoặc 30 phút tùy theo Ngày và Rạp
        DECLARE @StartOffset INT = ((DATEPART(DAY, @CurrentDate) + @RoomID) * 10) % 40;
        DECLARE @CurrentTime DATETIME = DATEADD(MINUTE, @StartOffset, CAST(@CurrentDate AS DATETIME) + CAST('09:00:00' AS DATETIME));
        
        -- TẠO SỰ KHÁC BIỆT: Phim chiếu suất đầu tiên của rạp sẽ thay đổi theo từng ngày
        DECLARE @MovieID INT = ((@RoomID + DATEPART(DAY, @CurrentDate)) % 10) + 1; 

        -- Chạy các suất trong ngày cho đến trước 23h30
        WHILE @CurrentTime <= (CAST(@CurrentDate AS DATETIME) + CAST('23:30:00' AS DATETIME))
        BEGIN
            DECLARE @Duration INT;
            SELECT @Duration = Duration FROM Movies WHERE MovieID = @MovieID;
            IF @Duration IS NULL SET @Duration = 120; 

            DECLARE @Price DECIMAL(18,0) = 50000;
            IF @RoomID IN (6, 7) SET @Price = 90000;

            INSERT INTO ShowTimes (MovieID, RoomID, StartTime, AdMinutes, TicketPrice, Duration)
            VALUES (@MovieID, @RoomID, @CurrentTime, @AdMinutes, @Price, @Duration);

            SET @CurrentTime = DATEADD(MINUTE, @AdMinutes + @Duration + @GapMinutes, @CurrentTime);

            -- Xoay vòng qua phim tiếp theo
            SET @MovieID = @MovieID + 1;
            IF @MovieID > 10 SET @MovieID = 1;
        END

        SET @RoomID = @RoomID + 1;
    END

    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT N'ĐÃ TẠO LẠI THÀNH CÔNG LỊCH CHIẾU! MỖI NGÀY SẼ LÀ MỘT LỊCH HOÀN TOÀN KHÁC NHAU!';

-- Thêm dữ liệu các vé đã đặt (booking)
USE CinemaDB; -- Nhớ đổi thành tên database của bạn
GO

-- 1. XÓA SẠCH VÉ CŨ ĐỂ BẮT ĐẦU GIẢ LẬP LẠI
DELETE FROM Bookings;
IF OBJECT_ID('Bookings', 'U') IS NOT NULL 
    DBCC CHECKIDENT ('Bookings', RESEED, 0);
GO

-- Biến cờ đánh dấu đã tạo suất Full rạp cho ngày 5/4 chưa
DECLARE @FullShowCreated BIT = 0; 
DECLARE @ShowID INT, @RoomID INT, @StartTime DATETIME, @BasePrice DECIMAL(18,0);

-- Tạo bảng tạm bên ngoài vòng lặp để tăng tốc độ xử lý
CREATE TABLE #TempSeats (RowIndex INT, ColIndex INT, SeatCode VARCHAR(10));

-- Lấy tất cả các suất chiếu từ ngày 1/4 đến 8/4
DECLARE curShow CURSOR FOR
SELECT ShowID, RoomID, StartTime, TicketPrice
FROM ShowTimes
WHERE CAST(StartTime AS DATE) BETWEEN '2026-04-01' AND '2026-04-08';

OPEN curShow;
FETCH NEXT FROM curShow INTO @ShowID, @RoomID, @StartTime, @BasePrice;

WHILE @@FETCH_STATUS = 0
BEGIN
    TRUNCATE TABLE #TempSeats; -- Làm sạch bảng tạm cho suất chiếu mới

    DECLARE @TotalSeats INT, @Cols INT, @Rows INT = 10; -- Số hàng mặc định là 10 theo C#
    SELECT @TotalSeats = TotalSeats FROM Rooms WHERE RoomID = @RoomID;
    
    -- Logic cột giống hệt C#
    IF @RoomID = 3 SET @Cols = 14;
    ELSE IF @RoomID >= 4 SET @Cols = 16;
    ELSE SET @Cols = 10;
    
    -- XÁC ĐỊNH SỐ LƯỢNG VÉ CẦN MUA
    DECLARE @TicketsToBook INT = 0;

    -- Điều kiện chèn suất FULL kín rạp ngày 5/4 (chọn suất tối sau 18h)
    IF CAST(@StartTime AS DATE) = '2026-04-05' AND CAST(@StartTime AS TIME) >= '18:00:00' AND @FullShowCreated = 0
    BEGIN
        SET @TicketsToBook = @TotalSeats;
        SET @FullShowCreated = 1;
    END
    ELSE IF CAST(@StartTime AS TIME) < '15:00:00'
    BEGIN
        -- Buổi sáng (trước 15h): Random từ 10 đến 15 vé
        SET @TicketsToBook = 10 + ABS(CHECKSUM(NEWID())) % 6; 
    END
    ELSE
    BEGIN
        -- Buổi tối (sau 15h): Random từ 20 đến 30 vé
        SET @TicketsToBook = 20 + ABS(CHECKSUM(NEWID())) % 11; 
    END
    
    -- An toàn: Không để số vé mua vượt quá tổng số ghế của rạp
    IF @TicketsToBook > @TotalSeats SET @TicketsToBook = @TotalSeats;

    -- TẠO SƠ ĐỒ GHẾ ẢO ĐỂ BỐC THĂM
    DECLARE @r INT = 0, @SeatCounter INT = 0;
    WHILE @r < @Rows
    BEGIN
        DECLARE @c INT = 0;
        WHILE @c < @Cols
        BEGIN
            SET @SeatCounter = @SeatCounter + 1;
            IF @SeatCounter <= @TotalSeats
            BEGIN
                INSERT INTO #TempSeats (RowIndex, ColIndex, SeatCode)
                VALUES (@r, @c, CHAR(65 + @r) + CAST((@c + 1) AS VARCHAR(10)));
            END
            SET @c = @c + 1;
        END
        SET @r = @r + 1;
    END

    -- LẤY NGẪU NHIÊN GHẾ VÀ CHỐT ĐƠN (INSERT VÀO BOOKINGS)
    INSERT INTO Bookings (ShowID, RoomID, SeatNumber, Price, BookingTime, Status, AccountID)
    SELECT TOP (@TicketsToBook)
        @ShowID,
        @RoomID,
        SeatCode,
        CASE 
            WHEN RowIndex >= (@Rows - 3) THEN @BasePrice + 10000 -- CỘNG 10K CHO 3 HÀNG VIP CUỐI
            ELSE @BasePrice 
        END,
        -- Giả lập giờ đặt vé là trước giờ chiếu từ 15 đến 300 phút
        DATEADD(MINUTE, -(15 + ABS(CHECKSUM(NEWID())) % 285), @StartTime), 
        'PAID',
        1 -- Mặc định ID nhân viên bán là 1
    FROM #TempSeats
    ORDER BY NEWID(); -- Sắp xếp ngẫu nhiên để chọn ghế lộn xộn, tự nhiên nhất

    FETCH NEXT FROM curShow INTO @ShowID, @RoomID, @StartTime, @BasePrice;
END

CLOSE curShow;
DEALLOCATE curShow;
DROP TABLE #TempSeats;

PRINT N'ĐÃ GIẢ LẬP XONG DỮ LIỆU BÁN VÉ TỪ 01/04 - 08/04 THÀNH CÔNG (CÓ SUẤT FULL KÍN GHẾ NGÀY 5/4)!';

-- Thêm dữ liệu Account
USE CinemaDB; -- Đổi thành tên database thực tế của bạn
GO

-- 1. XÓA SẠCH TÀI KHOẢN CŨ TRƯỚC KHI THÊM
-- (Lưu ý: Nếu bị lỗi do vướng khóa ngoại với bảng Bookings, bạn phải chạy lệnh DELETE FROM Bookings trước nhé)
DELETE FROM Account;
GO

-- 2. BẬT CHẾ ĐỘ ÉP NHẬP ID BẰNG TAY
SET IDENTITY_INSERT Account ON;
GO

-- 3. THÊM DỮ LIỆU 
INSERT INTO Account (AccountID, Username, Password, FullName, Role)
VALUES 
(1, 'admin', '123456', N'Quản trị viên Hệ thống', 'Admin'),
(2, 'nhanvien1', '123', N'Nguyễn Lê Anh Trúc', 'Employee'),
(5, 'Thao An', '123', N'Đinh Thị Thảo An', 'Employee'),
(6, 'Luan', '123', N'Lưu Gia Luân', 'Employee');
GO

-- 4. TẮT CHẾ ĐỘ ÉP NHẬP ID TRẢ LẠI BÌNH THƯỜNG
SET IDENTITY_INSERT Account OFF;
GO

PRINT N'ĐÃ THÊM THÀNH CÔNG DATA BẢNG TÀI KHOẢN CHUẨN XÁC 100%!';
SELECT * FROM Account; -- Hiển thị lên để bạn kiểm tra luôn