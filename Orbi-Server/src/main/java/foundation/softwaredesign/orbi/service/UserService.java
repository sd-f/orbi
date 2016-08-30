package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.model.auth.LoginInfo;
import foundation.softwaredesign.orbi.model.auth.RequestCodeInfo;
import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import foundation.softwaredesign.orbi.persistence.repo.IdentityRepository;
import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import org.apache.commons.lang3.RandomStringUtils;

import javax.annotation.Resource;
import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.mail.Address;
import javax.mail.Message;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.persistence.NoResultException;
import javax.validation.Valid;
import javax.validation.constraints.NotNull;
import javax.ws.rs.InternalServerErrorException;
import java.util.Date;
import java.util.Objects;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class UserService {

    private AuthorizationInfo authorizationInfo = new AuthorizationInfo();
    private IdentityEntity identityEntity = null;

    @Resource(mappedName = "java:comp/env/SDFMail")
    private Session smtpSession;

    @Inject
    IdentityRepository identityRepository;

    public void setIdentityEntity(IdentityEntity identityEntity) {
        this.identityEntity = identityEntity;
    }
    public IdentityEntity getIdentity() {
        return this.identityEntity;
    }

    public AuthorizationInfo getAuthorizationInfo() {
        return this.authorizationInfo;
    }

    public void requestPassword(@Valid @NotNull RequestCodeInfo requestCodeInfo) {
        IdentityEntity identityEntity = identityRepository.findByEmail(requestCodeInfo.getEmail());
        if (Objects.isNull(identityEntity)) {
            identityEntity = new IdentityEntity();
            identityEntity.setEmail(requestCodeInfo.getEmail());
        }


        setPlayerPosition(identityEntity, requestCodeInfo.getPlayer().getGeoPosition());
        identityEntity.setLastSeen(new Date());

        String password = RandomStringUtils.randomAlphanumeric(20).toUpperCase();
        identityEntity.setTmpPassword(new ChkPass(password));

        saveUser(identityEntity);
        if (!sendPasswordMail(identityEntity.getEmail(), password)) {
            throw new InternalServerErrorException("Email could not be send");
        }
    }

    public AuthorizationInfo login(@Valid @NotNull LoginInfo loginInfo) {

        IdentityEntity identityEntity = identityRepository.findByEmail(loginInfo.getEmail());
        if (Objects.isNull(identityEntity)) {
            throw new InternalServerErrorException("Email not registered");
        }
        try {
            Long id = identityRepository.findIdentityIdByEmailAndPassword(identityEntity.getEmail(), loginInfo.getPassword());
            identityEntity = identityRepository.findBy(id);
        } catch (NoResultException ex) {
            throw new InternalServerErrorException("Email or Password incorrect");
        }

        setPlayerPosition(identityEntity, loginInfo.getPlayer().getGeoPosition());
        identityEntity.setLastSeen(new Date());

        String token = RandomStringUtils.randomAlphanumeric(100);
        identityEntity.setToken(new ChkPass(token));

        AuthorizationInfo authorizationInfo = new AuthorizationInfo();
        authorizationInfo.setToken(token);

        saveUser(identityEntity);
        return authorizationInfo;
    }

    public void updatePosition(GeoPosition position) {
        setPlayerPosition(identityEntity, position);
        saveUser(identityEntity);
    }


    private void setPlayerPosition(IdentityEntity identity, GeoPosition psoition) {
        identity.setLatitude(psoition.getLatitude());
        identity.setLongitude(psoition.getLongitude());
        identity.setElevation(psoition.getAltitude());
        identity.setRotationX(new Double(0));
        identity.setRotationY(new Double(0));
    }

    private void saveUser(IdentityEntity identity) {
        identityRepository.save(identity);
    }

    private Boolean sendPasswordMail(String email, String password) {

        final Message message = new MimeMessage(smtpSession);
        try {
            message.setRecipients(Message.RecipientType.TO, new Address[]{
                    new InternetAddress(email)
            });
            message.setFrom(new InternetAddress("orbi@softwaredesign.foundation"));
            message.setSubject("Code for Orbi");
            message.setSentDate(new Date());
            message.setText("This is your code: " + password);
            Transport.send(message);
        } catch (Exception e) {
            Logger.getLogger(this.getClass().getName()).log(Level.SEVERE, null, e);
            return false;
        }
        return true;
    }

}
