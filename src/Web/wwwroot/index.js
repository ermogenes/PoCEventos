let produtos, vendas;

const obtemProdutos = async () => {
    const produtosResponse = await fetch('./api/produtos');
    return produtosResponse.ok ? await produtosResponse.json() : [];
}

const obtemVendas = async () => {
    const vendasResponse = await fetch('./api/vendas');
    return vendasResponse.ok ? await vendasResponse.json() : [];
}

const atualizaListaProdutos = async () => {
    produtos = await obtemProdutos();

    const listaProdutos = document.getElementById("lista-produtos").tBodies[0];
    const selectProdutos = document.getElementById("produto-id");
    listaProdutos.innerHTML = "";
    Array.from(produtos).forEach(p => {
        const marcacaoProduto = `<tr><td>${p.id}</td><td>${p.descricao}</td><td>${p.precobrl}</td></tr>`;
        listaProdutos.insertAdjacentHTML('beforeend', marcacaoProduto);

        const optionProduto =  `<option value="${p.id}">${p.descricao} - ${p.precobrl}</option>`;
        selectProdutos.insertAdjacentHTML('beforeend', optionProduto);
    });
}
const atualizaListaVendas = async () => {
    vendas = await obtemVendas();

    const listaVendas = document.getElementById("lista-vendas").tBodies[0];
    listaVendas.innerHTML = "";
    Array.from(vendas).forEach(v => {
        const marcacaoVenda = `<tr><td>${v.id}</td><td>${v.produtoId} ${v.produto.descricao}</td><td>${v.quantidade}</td><td>${v.precobrl}</td><td>${v.cotacaousd}</td><td>${v.taxabrl}</td><td>${v.totalbrl}</td></tr>`;
        listaVendas.insertAdjacentHTML('beforeend', marcacaoVenda);
    });
}

const atualizaCotacao = async () => {
    const response = await fetch('https://economia.awesomeapi.com.br/json/last/USD-BRL');
    if (response.ok) {
        const cotacao = document.getElementById("cotacao-atual");
        const dados = await response.json();
        cotacao.innerHTML = `1 BRL = ${dados.USDBRL.bid} USD`;
    }
};

const enviaPedido = async (evt) => {
    evt.preventDefault();
    const mensagem = document.getElementById("mensagem");
    const selectProdutos = document.getElementById("produto-id");
    const produtoId = selectProdutos.options[selectProdutos.selectedIndex].value;
    const quantidade = document.getElementById("quantidade").value;
    const novoPedido = { produtoId, quantidade };
    const response = await fetch("/api/pedidos", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(novoPedido),
    });
    if (response.ok) {
        mensagem.innerHTML = "Pedido recebido.";
    } else {
        mensagem.innerHTML = "Erro ao enviar pedido.";
    }
    setInterval(() => {mensagem.innerHTML = ""; }, 2000);
};

const exibeNotificacao = (origem, mensagem) => {
    const lista = document.getElementById("notificacoes");
    const notificacao = document.createElement('div');
    notificacao.innerHTML = `${origem}: ${mensagem} (${new Date().toLocaleTimeString()})`;
    notificacao.classList.add("notificacao-mensagem");
    lista.append(notificacao);
    setTimeout(() => { notificacao.style.display = "none"; }, 5000);
};

const limpaNotificacao = () => {
    document.getElementById("notificacoes").innerHTML = "";
};

document.addEventListener('DOMContentLoaded', async () => {
    await atualizaListaProdutos();
    await atualizaCotacao();
    
    setInterval(atualizaCotacao, 5000);
    setInterval(atualizaListaVendas, 2000);

    document.querySelectorAll('.atualizar-lista-produtos').forEach(el => el.addEventListener('click', () => atualizaListaProdutos()));
    document.querySelectorAll('.atualizar-lista-vendas').forEach(el => el.addEventListener('click', () => atualizaListaVendas()));
    document.querySelectorAll('.atualizar-cotacao').forEach(el => el.addEventListener('click', () => atualizaCotacao()));

    document.getElementById('limpa-notificacao').addEventListener('click', (evt) => limpaNotificacao(evt));

    document.getElementById('enviar-pedido').addEventListener('click', (evt) => enviaPedido(evt));
});