// Darkmode měnění stylu
function darkMode(){
    // tady něco chybí (změňte classList celého těla dokumentu a formuláře - nějaké stylování už je předdělané v CSS souboru)
    
    document.body.classList.toggle("dark-mode");
    document.getElementById("form").classList.toggle("dark-mode");

    const inputElements = document.getElementsByTagName("input");
    for (const input of inputElements) {
        input.classList.toggle("dark-mode");
    }

    document.getElementById("darkmodebtn").classList.toggle("dark-mode");
}

// Ověření stejnosti hesel
function passwordsMatch() {
    // tady něco chybí (získejte pwd1 a pwd2)
    
    var pwd1 = document.getElementById("pwd").value;
    var pwd2 = document.getElementById("pwd-repeat").value;

    if (pwd1 !== pwd2){
        document.getElementById("pwd-alert").style.display="block";
    }
    else{
        document.getElementById("pwd-alert").style.display="none";
    }
}

