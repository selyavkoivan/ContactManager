$(document).ready(() => {

    HideAlert()
    
    function HideAlert() {
        $("#birthday-alert").hide()
        $("#name-alert").hide()
        $("#job-alert").hide()
        $("#phone-alert").hide()
        $(".EditContact").hide()
        $(".DeleteContact").hide()
    }
    
    $("#AddContact").click(() => {
        $("#modalLabel").text("Add Contact")
        CleanForm()
        $("#Add").show()
        $("#Update").hide()
    })
    
    $(".EditContact").click(e => {
        let id = $(e.target).closest("button").attr("id")
        $("#ContactId").val(id)
        $("#modalLabel").text("Edit Contact")

        $.ajax({
            url: url.Read + id,
            method: "GET",
            success: (data) => {
                FillForm(data)
                $("#Add").hide()
                $("#Update").show()
            }
        })
    })
    
    $('.DeleteContact').click(e => {
        $.ajax({
            url: url.Delete + $(e.target).closest("button").attr("id") + GetCurrentPageUrl(),
            method: "DELETE",
            success: data => {
                if(data) {
                    location.replace("/" + GetPrevPageUrl())
                }
                else {
                    location.replace("/" + GetCurrentPageUrl())
                }
                
            }
        })
    })

    function CleanForm() {
        $("#contact-name").val('')
        $("#phone").val('')
        $("#birthday").val('')
        $('#job-title').val('')
    }
    
    function FillForm(contact)
    {
        $("#contact-name").val(contact.name)
        $("#phone").val(contact.phone)
        $("#birthday").val(contact.birthday.substr(0, 10))
        $('#job-title').val(contact.position.jobTitle)
    }
    
    $("#Add").click(() => {
        const formData = GetDataFromPage()
        if(isValid(formData)) {
           
            $.ajax({
                url: url.Create,
                method: "POST",
                data: JSON.stringify(formData),
                contentType: "application/json;charset=utf-8",
                success: data => {
                    location.replace("/" + GetPageUrl(data))
                }
            })
        }

    })

    $("#Update").click(() => {
        const data = GetDataFromPage()
        if(isValid(data)) {
            $.ajax({
                url: url.Update + data.ContactId,
                method: "PUT",
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: () => {
                    location.reload()
                }
            })
        }
    })
    
    function GetDataFromPage() {
        return {
            Name: $("#contact-name").val(),
            Phone: $("#phone").val(),
            JobTitle: $("#job-title").val(),
            Birthday: $("#birthday").val(),
            ContactId: $("#ContactId").val()
        }
    }
    
    function isValid(data) {
        let isValid = true
        if(data.Phone.match(regex.Phone) == null) {
            $("#phone-alert").show()
            isValid = false
        }
        if(new Date(data.Birthday) >= new Date()) {
            $("#birthday-alert").show()
            isValid = false
        }
        if(data.JobTitle.match(regex.JobTitle) == null) {
            $("#job-alert").show()
            isValid = false
        }
        if(data.Name.match(regex.Name) == null) {
            $("#name-alert").show()
            isValid = false
        }
        return isValid
    }
    
    $(".btn-close").click(e => {
        $(e.target).closest("div .alert").hide()
    })
    
    $("#prev").click(e => {
        location.replace("/" + GetPrevPageUrl())
    })

    $("#next").click(e => {
        location.replace("/" + GetNextPageUrl())
    })

    function GetCurrentPageUrl() {
        return "?page=" + $("#page").val()
    }

    function GetPageUrl(page) {
        return "?page=" + page
    }
    
    function GetPrevPageUrl() {
        return "?page=" + (+$("#page").val() - 1)
    }

    function GetNextPageUrl() {
        return "?page=" + (+$("#page").val() + 1)
    }
    
    $("[name='contactRadio']").click(e => {
        let id = $(e.target).attr("id")
        $(".EditContact").attr("id", id).show()
        $(".DeleteContact").attr("id", id).show()
    })

})