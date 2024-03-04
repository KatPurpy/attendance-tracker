(function () {
    let studentCache = undefined;
    function load()
    {
        return [...document.getElementsByClassName("studentCells")]
    }

    // this lookup algorithm searches by returning first name that matches the input
    // it's enough for <=30 students per class
    let LookupStudentName = function (query) {
        if (studentCache === undefined) {
            studentCache = load();
        }
        let lowerCaseQuery = query.toLowerCase();
        return query ? studentCache.find((el) => el.innerText.toLowerCase().includes(lowerCaseQuery)) : undefined;
    };

    let LookupNameAndCell = function (str) {
        let name = LookupStudentName(str);
        if (!name) {
            return undefined;
        }
        let day = document.getElementById("fastInput_date").value;
        let studentId = name.getAttribute("student");

        let query = `input.entry-cell[student="${studentId}"][date*="${day}"]`;
        let node = document.querySelector(query);

        return [name, node];
    };

    let date = new Date();
    document.getElementById("fastInput_date").value = date.getFullYear() + '-' + (date.getMonth()+1).toString().padStart(2, '0') + '-' + date.getDay().toString().padStart(2, '0')

    document.getElementById("fastInput_input").addEventListener("keypress", function (event) {
        // If the user presses the "Enter" key on the keyboard
        if (event.key === "Enter") {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            UpdateCurrentCellValue(true);

            this.value = "";
        }
    }); 

    window.UpdateCurrentCellValue = function (modify) {
        let input = document.getElementById("fastInput_input").value.split(' ');
        let name, cell;
        [name, cell] = LookupNameAndCell(input[0]);

        if (modify) {
            cell.value = input[1] || "";
            CommitCell(cell);
        }
        OnFastInputChange(input[0]);
    }

    window.OnFastInputChange = function (str) {
        let name, cell;
        [name, cell] = LookupNameAndCell(str);

        if (name && cell) {
            document.getElementById("fastInput_previewStudent").innerText = name.innerText;
            document.getElementById("fastInput_previewCellValue").innerText = cell.value || "empty";
        } else {
            document.getElementById("fastInput_previewStudent").innerText = "not found";
            document.getElementById("fastInput_previewCellValue").innerText = "not found";
        }
    }
})();