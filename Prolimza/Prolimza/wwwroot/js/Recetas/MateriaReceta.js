let index = @Model.MateriasReceta.Count;

function addMateriaReceta() {
    const container = document.getElementById("materiaRecetaContainer");

    const div = document.createElement("div");
    div.classList.add("materia-receta-row");
    div.innerHTML = `
            <select name="MateriasReceta[${index}].IdMateriaPrima" class="form-control-custom">
                ${@Html.Raw(Json.Serialize(ViewBag.MateriaPrimas)).Replace("\"", "&quot;")}
            </select>
            <input name="MateriasReceta[${index}].Cantidad" class="form-control-custom" placeholder="Cantidad" />
        `;
    container.appendChild(div);
    index++;
}