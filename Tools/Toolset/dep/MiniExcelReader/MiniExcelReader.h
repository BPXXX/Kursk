#ifndef _TINYXLSX_H_
#define _TINYXLSX_H_

#include <vector>
#include <string>
namespace MiniExcelReader
{
    struct Cell
    {
        std::string value;
        std::string type;
    };

    struct Range
    {
        int firstRow;
        int lastRow;
        int firstCol;
        int lastCol;
    };

    class Sheet
    {
    public:
        ~Sheet();

        bool visible() { return _visible; }
        const std::string& getName() { return _name; }
        Range& getDimension() { return _dimension; }

        Cell* getCell(int row, int col);
    private:
        friend class ExcelFile;

        int toIndex(int row, int col);

        int _sheetId;
        bool _visible;
        Range _dimension;

        std::string _rid;
        std::string _path;
        std::string _name;

        std::vector<Cell*> _cells;
		bool _loaded;
    };

    class Zip;

    class ExcelFile
    {
    public:
        ~ExcelFile();
        bool open(const char* filename, bool autoLoad = true);

        Sheet* getSheet(const char* name);
        std::vector<Sheet>& sheets() { return _sheets; }
        void loadSheet(Sheet& sh);
    private:

        void readWorkBook(const char* filename);
        void readWorkBookRels(const char* filename);
        void readSharedStrings(const char* filename);
        void readStyles(const char* filename);
        void readSheet(Sheet& sh);

        void parseCell(const std::string& value, int& row, int& col);
        void parseRange(const std::string& value, Range& range);

        std::vector<std::string> _sharedString;
        std::vector<Sheet> _sheets;
        Zip* _zip;
    };
}


#endif
