function convertFormToObject(form) {
    const array = $(form).serializeArray(); // Encodes the set of form elements as an array of names and values.
    const json = {};
    $.each(array, function () {
        if (this.value && this.value != "") {
            json[this.name] = this.value;
        }
    });
    return json;
}

function CreateCrudObj(formName, type) {
    let formData = convertFormToObject(formName);
    console.log(formData);
    $.ajax({
        type: "POST",
        url: `/api/${type}/Create`,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(formData),
        success: () => {
            location.reload();
        }
    });
}

function ChangeCrudObj(formName, type) {
    let formData = convertFormToObject(formName);
    console.log(formData);
    $.ajax({
        type: "POST",
        url: `/api/${type}/Update?id=` + formData.Id,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(formData),
        success: () => {
            location.reload();
        }
    });
}
function LoadCrudObj(formname, type, id) {
    $.ajax({
        type: "GET",
        url: `/api/${type}/Read`,
        data: { id },
        success: function (result) {
            console.log(result);
            for (let key in result) {
                let query = formname + ` input[name="${key}" i]`;
                console.log(query);
                var form = $(query);
                console.log(form);
                form.val(result[key]);
            }
        }

    });
}
function DeleteCrudObj(type, id) {
    $.ajax({
        type: "POST",
        url: `/api/${type}/Delete`,
        data: { id },
        success: function (result) {
            location.reload();
        }
    });
}