<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerAdd.ascx.cs" Inherits="ServicePoint.Common.UC.Modal.ServerAdd" %>
<script type="text/javascript">
    function AddServer() {

        var frm = $('#frmProc');
        var $container = $('#_AddServer');
        if (!$container.find('#strDisplayName').val()) {
            $container.find('#strDisplayName').focus();
            return;
        }
        if (!$container.find('#strDisplayGroup').val()) {
            $container.find('#strDisplayGroup').focus();
            return;
        } if (!$container.find('#strServerType').val()) {
            $container.find('#strServerType').focus();
            return;
        } if (!$container.find('#strLanguage').val()) {
            $container.find('#strLanguage').focus();
            return; 
        } 

        $('#command').val('addserver');
        $('#hdn_strDisplayName').val($container.find('#strDisplayName').val());
        $('#hdn_strDisplayGroup').val($container.find('#strDisplayGroup').val());
        $('#hdn_strServerType').val($container.find('#strServerType').val());
        $('#hdn_strLanguage').val($container.find('#strLanguage').val());

        var strBtnSubmit = $container.find('#btnSubmit').val();
        $container.find('#btnSubmit').val('Sending...').attr('disabled', 'disabled');


        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
                $container.find('#btnSubmit').val(strBtnSubmit).removeAttr('disabled');
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function UpdateServer() {

        var frm = $('#frmProc');
        var $container = $('#_UpdateServer');
       

        $('#command').val('updateserver');
        $('#hdn_strDisplayName').val($container.find('#strDisplayName').val());
        $('#hdn_strDisplayGroup').val($container.find('#strDisplayGroup').val());
        $('#hdn_strServerType').val($container.find('#strServerType').val());
        $('#hdn_strLanguage').val($container.find('#strLanguage').val());


        var strBtnSubmit = $container.find('#btnSubmit').val();
        $container.find('#btnSubmit').val('Sending...').attr('disabled', 'disabled');


        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
                $container.find('#btnSubmit').val(strBtnSubmit).removeAttr('disabled');
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function DelServer(numServer) {

        var $frm = $('#frmProc');
        if (confirm("삭제하시겠습니까?") != true) {
            return;
        }
        $('#command').val('delserver');
        $frm.find('#hdn_numServer').val(numServer);

        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function modalAddServer() {

        var $frm = $('#frmProc');
        var $container = $('#_AddServer');
        $('#command').val('addserver');
        $container.find('#strDisplayName').val('');
        $container.find('#strDisplayGroup').val('');
        $container.find('#strServerType').val('');
        $container.find('#strLanguage').val('');
         $container.find('#btnSubmit').removeAttr('disabled');
        //$frm.find('#hdn_numWorld').val(numWorld);
        //$frm.find('#hdn_oidAccount').val(oidAccount);
        //$frm.find('#numPoint_Mileage').val(numPoint_Mileage);
        //$container.find('#oidAccount').val(oidAccount).attr("readonly", true);;
        //$container.find('#numPoint_Mileage').val(numPoint_Mileage);

        $('#frmProc').submit(function (e) {
            e.preventDefault();
        });
        $container.modal();
    }
    function modalUpdateServer(strDisplayName, strDisplayGroup,strServerType, numServer, strLanguage) {

        var $frm = $('#frmProc');
        var $container = $('#_UpdateServer');
        $('#command').val('updateserver');
        $container.find('#strDisplayName').val(strDisplayName);
        $container.find('#strDisplayGroup').val(strDisplayGroup);
        $container.find('#strServerType').val(strServerType);
        $frm.find('#hdn_numServer').val(numServer);
        $container.find('#strLanguage').val(strLanguage);
        $container.find('#btnSubmit').removeAttr('disabled');
        $container.find('#strServerType').attr('readonly', true);
        $container.find('#strServerType').attr('disabled', true);
        //$frm.find('#hdn_numWorld').val(numWorld);
        //$frm.find('#hdn_oidAccount').val(oidAccount);
        //$frm.find('#numPoint_Mileage').val(numPoint_Mileage);
        //$container.find('#oidAccount').val(oidAccount).attr("readonly", true);;
        //$container.find('#numPoint_Mileage').val(numPoint_Mileage);

        $('#frmProc').submit(function (e) {
            e.preventDefault();
        });
        $container.modal();
    }
</script>
<form id="frmProc" name="frmProc" method="post" class="form-horizontal">
    <input type="hidden" id="command" name="command" />
    <input type="hidden" id="hdn_MemberNum" name="hdn_MemberNum" />
    <input type="hidden" id="hdn_CompanyNum" name="hdn_CompanyNum" />
    <input type="hidden" id="hdn_strDisplayName" name="hdn_strDisplayName" />
    <input type="hidden" id="hdn_strDisplayGroup" name="hdn_strDisplayGroup" />
    <input type="hidden" id="hdn_strServerType" name="hdn_strServerType" />
    <input type="hidden" id="hdn_strLanguage" name="hdn_strLanguage" />
    <input type="hidden" id="hdn_numServer" name="hdn_numServer" />
</form>
<div class="modal fade form-horizontal" id="_AddServer">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-info">
                <a class="close" data-dismiss="modal">×</a>
                <h3>서버 추가</h3>
            </div>
            <div class="modal-body">
                <fieldset>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strDisplayName">
                            서버명
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strDisplayName" name="strDisplayName" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strDisplayGroup">
                           그룹명
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strDisplayGroup" name="strDisplayGroup" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strServerType">
                            서버유형
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                             <select id="strServerType" name="strServerType" class="form-control">
                                <option value="SQL">SQL</option>
                              <%--  <option value="Web">Web</option>
                                <option value="Windows">Windows</option>--%>
                            </select>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strLanguage">
                            사용언어
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="strLanguage" name="strLanguage" class="form-control">
                                <option value="KR">한국어</option>
                                <option value="EN">English</option>
                                <option value="JP">日本語</option>
                            </select>
                        </div>
                    </div>
                </fieldset>

            </div>
            <div class="modal-footer">
                <a data-dismiss="modal" class="btn btn-small btn-warning">닫기</a>
                <input type="button" id="btnSubmit" class="btn btn-small btn-primary" value="추가"
                    onclick="AddServer();" />
            </div>
        </div>
    </div>
</div>
<div class="modal fade form-horizontal" id="_UpdateServer">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-info">
                <a class="close" data-dismiss="modal">×</a>
                <h3>서버 추가</h3>
            </div>
            <div class="modal-body">
                <fieldset>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strDisplayName">
                            서버명
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strDisplayName" name="strDisplayName" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strDisplayGroup">
                           그룹명
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strDisplayGroup" name="strDisplayGroup" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strServerType">
                            서버유형
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                             <select id="strServerType" name="strServerType" class="form-control">
                                <option value="SQL">SQL</option>
                                <option value="Web">Web</option>
                                <option value="Windows">Windows</option>
                            </select>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strLanguage">
                            사용언어
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="strLanguage" name="strLanguage" class="form-control">
                                <option value="KR">한국어</option>
                                <option value="EN">English</option>
                                <option value="JP">日本語</option>
                            </select>
                        </div>
                    </div>
                </fieldset>

            </div>
            <div class="modal-footer">
                <a data-dismiss="modal" class="btn btn-small btn-warning">닫기</a>
                <input type="button" id="btnSubmit" class="btn btn-small btn-primary" value="수정"
                    onclick="UpdateServer();" />
            </div>
        </div>
    </div>
</div>
