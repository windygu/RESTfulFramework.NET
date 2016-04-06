function setMiddle(element) {
    //取自已的高度
    var myHeight = $(element).height();
    //取父节点的高度
    var parentHeight = $(element).parent().height();
    //设置margin-top
    $(element).css("margin-top", (parentHeight / 2 - myHeight / 2) + "px");
}