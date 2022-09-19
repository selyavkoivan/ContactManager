const regex = {
    Name: /^[A-Za-zА-Яа-яЁё\- _]{1,50}$/,
    JobTitle: /^[A-Za-zА-Яа-яЁё0-9\- _]{1,50}$/,
    Phone: /^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$/
}

const url = {
    Create: "/Home/AddContact",
    Read: "/",
    Update: "/",
    Delete: "/"
}