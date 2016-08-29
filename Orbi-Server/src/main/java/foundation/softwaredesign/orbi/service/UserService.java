package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.model.auth.LoginUserInfo;
import foundation.softwaredesign.orbi.model.auth.RegisterUserInfo;
import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import foundation.softwaredesign.orbi.persistence.repo.UserRepository;
import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import foundation.softwaredesign.orbi.service.authorization.TokenThreadLocal;
import org.apache.commons.lang3.RandomStringUtils;

import javax.annotation.PostConstruct;
import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.persistence.EntityManager;
import javax.persistence.NoResultException;
import javax.persistence.PersistenceContext;
import javax.validation.Valid;
import javax.validation.constraints.NotNull;
import javax.ws.rs.InternalServerErrorException;
import javax.ws.rs.NotAuthorizedException;
import java.util.Date;
import java.util.Objects;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class UserService {

    private AuthorizationInfo authorizationInfo;
    private IdentityEntity identityEntity = null;

    @Inject
    UserRepository userRepository;

    @PostConstruct
    public void init() {
        this.authorizationInfo = new AuthorizationInfo();
        authorizationInfo.setToken(TokenThreadLocal.get());
        if (Objects.nonNull(authorizationInfo)
                && Objects.nonNull(authorizationInfo.getToken())
                && !authorizationInfo.getToken().isEmpty()) {
            try {
                Long id = userRepository.findIdentityIdByToken(authorizationInfo.getToken());
                this.identityEntity = userRepository.findBy(id);
            } catch (NoResultException ex) {
                throw new NotAuthorizedException("Token not valid");
            }
        }
    }

    public Boolean isLoggedIn() {
        return Objects.nonNull(identityEntity);
    }

    public AuthorizationInfo getAuthorizationInfo() {
        return this.authorizationInfo;
    }

    public void requestPassword(@Valid @NotNull RegisterUserInfo registerUserInfo) {

        if (Objects.nonNull(userRepository.findByEmail(registerUserInfo.getEmail()))) {
            throw new InternalServerErrorException("Email already registered");
        }

        IdentityEntity identityEntity = new IdentityEntity();

        identityEntity.setEmail(registerUserInfo.getEmail());
        setPlayerInfo(identityEntity, registerUserInfo.getPlayer());
        identityEntity.setLastSeen(new Date());

        String password = RandomStringUtils.randomAlphanumeric(20).toUpperCase();

        identityEntity.setTmpPassword(new ChkPass(password));

        userRepository.saveAndFlush(identityEntity);
    }

    public AuthorizationInfo login(@Valid @NotNull LoginUserInfo loginUserInfo) {
        IdentityEntity identityEntity = userRepository.findByEmail(loginUserInfo.getEmail());
        if (Objects.isNull(identityEntity)) {
            throw new InternalServerErrorException("Email not registered");
        }
        Long id = userRepository.findIdentityIdByEmailAndPassword(identityEntity.getEmail(), loginUserInfo.getPassword());
        try {
            identityEntity = userRepository.findBy(id);
        } catch (NoResultException ex) {
            throw new NotAuthorizedException("Token not valid");
        }

        setPlayerInfo(identityEntity, loginUserInfo.getPlayer());
        identityEntity.setLastSeen(new Date());

        String token = RandomStringUtils.randomAlphanumeric(100);
        identityEntity.setToken(new ChkPass(token));

        AuthorizationInfo authorizationInfo = new AuthorizationInfo();
        authorizationInfo.setToken(token);

        userRepository.saveAndFlush(identityEntity);
        return authorizationInfo;
    }

    private void setPlayerInfo(IdentityEntity identity, Player player) {
        identity.setLatitude(player.getGeoPosition().getLatitude());
        identity.setLongitude(player.getGeoPosition().getLongitude());
        identity.setElevation(player.getGeoPosition().getAltitude());
        identity.setRotationX(new Double(0));
        identity.setRotationY(new Double(0));
    }

}
