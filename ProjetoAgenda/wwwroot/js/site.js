// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function MedirSenha(password, meter, text) {
    var strength = {
        0: "Muito Fraca",
        1: "Fraca",
        2: "OK",
        3: "Boa",
        4: "Muito Boa"
    }

    password.addEventListener('input', function () {
        var val = password.value;
        var result = zxcvbn(val);

        // This updates the password strength meter
        meter.value = result.score;

        // This updates the password meter text
        if (val !== "") {
            text.innerHTML = strength[result.score];
            meter.style.visibility = "";
        } else {
            text.innerHTML = "";
            meter.style.visibility = "hidden";
        }
    });
}

function MascaraDeTelefone(telefone) {
    if (telefone.value) {
        const textoAtual = telefone.value;
        const isCelular = textoAtual.length === 11;
        let textoAjustado;
        if (isCelular) {
            const ddd = textoAtual.slice(0, 2);
            const parte1 = textoAtual.slice(2, 7);
            const parte2 = textoAtual.slice(7, 11);
            textoAjustado = `(${ddd}) ${parte1}-${parte2}`
        } else {
            const ddd = textoAtual.slice(0, 2);
            const parte1 = textoAtual.slice(2, 6);
            const parte2 = textoAtual.slice(6, 10);
            textoAjustado = `(${ddd}) ${parte1}-${parte2}`
        }

        telefone.value = textoAjustado;
    }
    else
        telefone.value = "";
}

function TiraHifen(telefone) {
    const textoAtual = telefone.value;
    const textoAjustado = textoAtual.replace('-', '').replace('(', '').replace(')', '').replace(' ', '');

    telefone.value = textoAjustado;
}
