function convertFormToObject(form) {
    let formData = new FormData(document.querySelector('#'+form));
    let result = Object.fromEntries(formData);
    for (let key in result)
    {
        if (!result[key] || result[key] == "") {
            delete result[key];
        }
    }
    return result;
}

async function CreateCrudObj(formName, type) {
    let formData = convertFormToObject(formName);
    console.log(formData);

    await fetch(`/api/${type}/Create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(formData)
    }).then(_ => location.reload());
}

async function ChangeCrudObj(formName, type) {
    let formData = convertFormToObject(formName);
    console.log(formData);

    await fetch(`/api/${type}/Update?` + new URLSearchParams({ id }), {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(formData)
    }).then(_ => location.reload());
}

async function LoadCrudObj(formname, type, id)
{
    const request = await fetch(`/api/${type}/Read?` + new URLSearchParams({ id }));
    result = await request.json();
    for (const key in result) {
        const qs = document.querySelector('#' + formname + ` input[name="${key}" i]`);
        qs.value = result[key];
    }
}

async function DeleteCrudObj(type, id) {
    await fetch(`/api/${type}/Delete?` + new URLSearchParams({ id }) , {
        method: "POST",
    }).then(_ => location.reload());

}