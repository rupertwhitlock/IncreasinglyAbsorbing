$(document).ready(function () {
    //debugger;
    $('#medias tbody').sortable({ update: Update }).disableSelection();
});

function Update(event, ui) {
    var medias = new Array();

    $(".name").each(function () {
        //        var imagePosition = new Object();
        //        imagePosition.Name = this.innerText;
        //        imagePosition.Position = this.parentElement.rowIndex;
        //        images.push(imagePosition);

        medias.push(this.innerText);
    });

    var ajaxData = ({ __RequestVerificationToken: $('[name=__RequestVerificationToken]').attr('value'),
        medias: medias, mediaGalleryName: $('#mediaGalleryName').val()
    });

    $.ajax({
        type: 'POST',
        data: ajaxData,
        url: 'Reorder',
        traditional: true
    });
}