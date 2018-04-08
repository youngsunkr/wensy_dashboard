<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleEditor.ascx.cs" Inherits="ServicePoint.Common.UC.Modal.RuleEditor" %>
<script type="text/javascript">

    function UpdateRule() {

        var frm = $('#frmProc');
        var $container = $('#_UpdateRule');

        $('#command').val('updatealertrules');

        $('#hdn_strPerformanceObject').val($container.find('#strPerformanceObject').val());
        $('#hdn_strCounter').val($container.find('#strCounter').val());
        $('#hdn_strLevel').val($container.find('#strLevel').val());
        $('#hdn_strDescription').val($container.find('#strDescription').val());
        $('#hdn_strInstance').val($container.find('#strInstance').val());
        $('#hdn_numThreshold').val($container.find('#numThreshold').val());
        $('#hdn_numDuration').val($container.find('#numDuration').val());
        $('#hdn_bolRecordApps').val($container.find('#bolRecordApps').val());
        $('#hdn_bolEnabled').val($container.find('#bolEnabled').val());
        $('#hdn_strMobileAlert').val($container.find('#strMobileAlert').val());


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

    function modalUpdateRule(PerformanceObject, Counter, Level, Description, Instance, Threshold, Duration, RecordApps, Enabled, MobileAlert,ReasonCode,numServer) {

        var $frm = $('#frmProc');
        var $container = $('#_UpdateRule');
        $('#command').val('updatealertrules');
        $('#hdn_numServer').val(numServer);
        $('#hdn_strReasonCode').val(ReasonCode);

        $container.find('#strPerformanceObject').val(PerformanceObject);
        $container.find('#strCounter').val(Counter);
        $container.find('#strLevel').val(Level);
        $container.find('#strDescription').val(Description);
        $container.find('#strInstance').val(Instance);
        $container.find('#numThreshold').val(Threshold);
        $container.find('#numDuration').val(Duration);
        $container.find('#bolRecordApps').val(RecordApps);
        $container.find('#bolEnabled').val(Enabled);
        $container.find('#strMobileAlert').val(MobileAlert);
        $container.find('#btnSubmit').removeAttr('disabled');

        $container.find('#strPerformanceObject').attr('readonly', true);
        $container.find('#strCounter').attr('readonly', true);
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
    <input type="hidden" id="hdn_bolRecordApps" name="hdn_bolRecordApps" />
    <input type="hidden" id="hdn_bolEnabled" name="hdn_bolEnabled" />
    <input type="hidden" id="hdn_strMobileAlert" name="hdn_strMobileAlert" />
    <input type="hidden" id="hdn_numDuration" name="hdn_numDuration" />
    <input type="hidden" id="hdn_numThreshold" name="hdn_numThreshold" />
    <input type="hidden" id="hdn_strInstance" name="hdn_strInstance" />
    <input type="hidden" id="hdn_strDescription" name="hdn_strDescription" />
    <input type="hidden" id="hdn_strLevel" name="hdn_strLevel" />
    <input type="hidden" id="hdn_strCounter" name="hdn_strCounter" />
    <input type="hidden" id="hdn_strPerformanceObject" name="hdn_strPerformanceObject" />
    <input type="hidden" id="hdn_numServer" name="hdn_numServer" />
    <input type="hidden" id="hdn_strReasonCode" name="hdn_strReasonCode" />
</form>
<div class="modal fade form-horizontal" id="_UpdateRule">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-info">
                <a class="close" data-dismiss="modal">×</a>
                <h3>사용자 추가</h3>
            </div>
            <div class="modal-body">
                <fieldset>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strPerformanceObject">
                            성능객체
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strPerformanceObject" name="strPerformanceObject" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strCounter">
                            성능카운터
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strCounter" name="strCounter" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strLevel">
                            알림수준
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="strLevel" name="strLevel" class="form-control">
                                <option value="Critical">Critical</option>
                                <option value="Warning">Warning</option>
                                <option value="Information">Information</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strDescription">
                            상세
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strDescription" name="strDescription" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strInstance">
                            인스턴스
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strInstance" name="strInstance" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="numThreshold">
                            임계값
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="numThreshold" name="numThreshold" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="numDuration">
                            수집기간
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="numDuration" name="numDuration" class="form-control">
                                <option value="0">0 - 즉시알림</option>
                                <option value="30">30초</option>
                                <option value="60">60초</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="bolRecordApps">
                            세부실행기록
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="bolRecordApps" name="bolRecordApps" class="form-control">
                                <option value="False">flase</option>
                                <option value="True">true</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="bolEnabled">
                            규칙활성화
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="bolEnabled" name="bolEnabled" class="form-control">
                                <option value="False">false</option>
                                <option value="True">true</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strMobileAlert">
                            모바일알림사용
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="strMobileAlert" name="strMobileAlert" class="form-control">
                                <option value="N">N</option>
                                <option value="Y">Y</option>
                            </select>
                        </div>
                    </div>
                </fieldset>

            </div>
            <div class="modal-footer">
                <a data-dismiss="modal" class="btn btn-small btn-warning">닫기</a>
                <input type="button" id="btnSubmit" class="btn btn-small btn-primary" value="수정"
                    onclick="UpdateRule();" />
            </div>
        </div>
    </div>
</div>
