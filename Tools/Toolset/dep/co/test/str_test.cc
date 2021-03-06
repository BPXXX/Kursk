#include "test.h"
#include "base/fastring.h"
#include "base/str.h"
#include <string>

int main(int argc, char** argv) {
    def_test(100000);

    fastring fs;
    std::string ss;

    fastring xfs = fastring("h") + fastring(100, 'e') + fastring("n");
    std::string xss = std::string("h") + std::string(100, 'e') + std::string("n");

    COUT << "\n======= copy =======";
    def_case(fs = fastring());
    def_case(ss = std::string());
    def_case(fs = "");
    def_case(ss = "");
    def_case(fs = xfs);
    def_case(ss = xss);

    COUT << "\n======= find =======";
    size_t r;
    def_case(r = xfs.find_first_of("no"));
    def_case(r = xss.find_first_of("no"));
    def_case(r = xfs.find_first_not_of("he"));
    def_case(r = xss.find_first_not_of("he"));
    def_case(r = xfs.find_last_of("hi"));
    def_case(r = xss.find_last_of("hi"));
    def_case(r = xfs.find_last_not_of("en"));
    def_case(r = xss.find_last_not_of("en"));
    def_case(r = xfs.find('n'));
    def_case(r = xss.find('n'));
    def_case(r = xfs.rfind('h'));
    def_case(r = xss.rfind('h'));
    def_case(r = xfs.rfind("hello"));
    def_case(r = xss.rfind("hello"));
    xfs = "0123456789";
    xss = "0123456789";
    def_case(r = xfs.rfind("012"));
    def_case(r = xss.rfind("012"));
    def_case(r = xfs.rfind("1234"));
    def_case(r = xss.rfind("1234"));
    def_case(r = xfs.rfind("34567"));
    def_case(r = xss.rfind("34567"));

    COUT << "\n======= substr =======";
    def_case(fs = xfs.substr(0));
    def_case(ss = xss.substr(0));
    def_case(fs = xfs.substr(3));
    def_case(ss = xss.substr(3));
    def_case(fs = xfs.substr(3, 3));
    def_case(ss = xss.substr(3, 3));
    def_case(fs = xfs.substr(8, 80));
    def_case(ss = xss.substr(8, 80));

    COUT << "\n======= append =======";
    fs.clear();
    ss.clear();
    def_case(fs.append("hello again"));
    def_case(ss.append("hello again"));

    fs.clear();
    ss.clear();

    COUT << "\n======= tolower/toupper =======";
    fs = "helloAgaiN";
    def_case(fs.toupper());
    def_case(fs.tolower());

    COUT << "\n======= replace/strip/match =======";
    def_case(fs.replace("ll", "yy"));
    def_case(fs.strip("hn"));

    fs = "helloagain";
    def_case(fs.match("*"));
    def_case(fs.match("hello*"));
    def_case(fs.match("*again"));
    def_case(fs.match("h*n"));
    def_case(fs.match("hell?again"));

    COUT << "\n======= to_string =======";
    def_case(str::from(12345678));
    def_case(std::to_string(12345678));
    def_case(fs = str::from(12345678));
    def_case(ss = std::to_string(12345678));
    def_case(str::from(3.14159));
    def_case(std::to_string(3.14159));
    def_case(fs = str::from(3.14159));
    def_case(ss = std::to_string(3.14159));

    return 0;
}
